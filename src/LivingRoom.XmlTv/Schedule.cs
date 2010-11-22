using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LivingRoom.XmlTv
{
    public class Schedule
    {


        public static void Import(string xmltvFileName, IDbConnection connection, string iconFolderPath)
        {
            var settings = new XmlReaderSettings()
                               {
                                   DtdProcessing = DtdProcessing.Ignore
                               };
            using (var streamReader = File.OpenText(xmltvFileName))
            {
                using (var xmlReader = XmlReader.Create(streamReader, settings))
                {
                    Import(xmlReader, connection, iconFolderPath);
                }
            }
        }

        public static void Import(XmlReader rdr, IDbConnection connection, string iconFolderPath)
        {
            rdr.MoveToContent();
            while (rdr.Read())
            {
                switch (rdr.LocalName)
                {
                    case "channel":
                        ImportChannel(rdr, connection, iconFolderPath);
                        break;
                    case "programme":
                        ImportProgram(rdr, connection);
                        break;
                }
            }
        }

        private static void ImportChannel(XmlReader rdr, IDbConnection connection, string iconFolderPath)
        {
            var doc = XDocument.Parse(rdr.ReadOuterXml());
            var root = doc.Root;
            if (root == null)
                return;

            var id = root.AttrValue("id");

            var names = root.Elements("display-name").Reverse();

            if (names.Count() < 3)
                return;

            var channel = (names.Where(name => name.Value.IsNumeric())
                               .FirstOrDefault() ?? new XElement("a")).Value;

            if (string.IsNullOrEmpty(channel))
                return;

            var longName = names.Skip(1).Take(1).Single().Value;
            var shortName = names.Skip(2).Take(1).Single().Value;
            if (shortName == longName)
                longName = names.Take(1).Single().Value;

            var iconUrl = root.SafeElement("icon").AttrValue("src");
            var iChannel = Convert.ToInt32(channel);
            if (string.IsNullOrWhiteSpace(iconUrl))
                iconUrl = FindIcon(iconFolderPath, iChannel);


            Database.WriteChannel(connection, id, channel, shortName, longName, iconUrl);
        }

        private static IDictionary<string, string> _iconFiles;
        private static string FindIcon(string iconFolderPath, int channel)
        {
            if (_iconFiles == null)
            {
                var files = Directory.GetFiles(iconFolderPath)
                    .Select(p => new {path = p, name = Path.GetFileName(p)});
                _iconFiles = files.ToDictionary(x => x.name, x => x.path);
            }

            var iconPath = _iconFiles
                .Where(kv => kv.Key.StartsWith(channel.ToString()))
                .Where(kv => !char.IsDigit(kv.Key[channel.ToString().Length]))
                .Select(kv => kv.Value)
                .FirstOrDefault();
            return iconPath ?? "";

        }

        private static void ImportProgram(XmlReader rdr, IDbConnection connection)
        {
            var doc = XDocument.Parse(rdr.ReadOuterXml());
            //(Id, ChannelId, StartTime, EndTime, 
            //Title, Description, EpisodeId, EpisodeNumber)
            var root = doc.Root;
            if (root == null)
                return;

            var id = Guid.NewGuid();
            var channel = root.AttrValue("channel");
            var startTime = ConvertDateTime(root.AttrValue("start"));
            if (!startTime.HasValue)
                return;
            var endTime = ConvertDateTime(root.AttrValue("stop"));
            if (!endTime.HasValue)
                return;
            var title = root.ElementValue("title", "en");
            if (string.IsNullOrWhiteSpace(title))
                return;

            var description = root.ElementValue("desc", "en");
            var episodeIdElem = root.Elements("episode-num")
                .FirstOrDefault(e => e.Attributes("system").Any(a => a.Value == "dd_progid"));
            var episodeId = episodeIdElem == null ? "" : episodeIdElem.Value;
            var episodeNumElem = root.Elements("episode-num")
                .FirstOrDefault(e => e.Attributes("system").Any(a => a.Value == "onscreen"));
            var episodeNum = episodeNumElem == null ? "" : episodeNumElem.Value;


            Database.WriteProgram(connection, id, channel, startTime.Value, endTime.Value,
                         title, description, episodeId, episodeNum);

            var categories = root.ElementValues("category", "en");
            foreach (var category in categories)
                Database.WriteCategory(connection, id, category);

            var credits = root.SafeElement("credits")
                .Elements().Select(e => new {role = e.Name.LocalName, name = e.Value});
            foreach (var credit in credits)
                Database.WriteCredit(connection, id, credit.role, credit.name);
        }

        private static DateTime? ConvertDateTime(string dateTime)
        {
            //20080715003000 -0600"
            DateTime ret;
            const string format = "yyyyMMddHHmmss zzz";
            return DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret)
                       ? (DateTime?) ret
                       : null;
        }
    }
}
