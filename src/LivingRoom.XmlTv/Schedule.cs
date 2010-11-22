using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NHibernate;

namespace LivingRoom.XmlTv
{
    public class Schedule
    {
        private readonly string _sourceFile;
        private readonly string _iconFolderPath;
        private IDictionary<string, string> _iconFiles;

        public Schedule(string sourceFile, string iconFolderPath)
        {
            _sourceFile = sourceFile;
            _iconFolderPath = iconFolderPath;
        }

        public void Export(ISession session)
        {
            var settings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Ignore
            };
            using (var streamReader = File.OpenText(_sourceFile))
            {
                using (var xmlReader = XmlReader.Create(streamReader, settings))
                {
                    Export(xmlReader, session);
                }
            }
        }

        private void Export(XmlReader rdr, ISession session)
        {
            rdr.MoveToContent();
            while (rdr.Read())
            {
                switch (rdr.LocalName)
                {
                    case "channel":
                        ExportChannel(rdr, session);
                        break;
                    case "programme":
                        ExportProgram(rdr, session);
                        break;
                }
            }
        }

        private void ExportChannel(XmlReader rdr, ISession session)
        {
            var root = GetElement(rdr);
            var channel = new Channel();
            
            channel.Id = root.AttrValue("id");
            ParseNames(root, channel);
            FindIcon(channel);

            using (var tx = session.BeginTransaction())
            {
                session.Save(channel);
                tx.Commit();
            }
            session.Clear();
        }

        private static void ParseNames(XElement root, Channel channel)
        {
            var names = root.Elements("display-name").Reverse();

            var number = (names.Where(name => name.Value.IsNumeric())
                              .FirstOrDefault() ?? new XElement("a")).Value;
            channel.Number = long.Parse(number);

            channel.LongName = names.Skip(1).Take(1).Single().Value;
            channel.ShortName = names.Skip(2).Take(1).Single().Value;
            if (channel.ShortName == channel.LongName)
                channel.LongName = names.Take(1).Single().Value;
        }

        private void FindIcon(Channel channel)
        {
            if (_iconFiles == null)
            {
                var files = Directory.GetFiles(_iconFolderPath)
                    .Select(p => new { path = p, name = Path.GetFileName(p) });
                _iconFiles = files.ToDictionary(x => x.name, x => x.path);
            }
            var iconPath = _iconFiles
                .Where(kv => kv.Key.StartsWith(channel.Number.ToString()))
                .Where(kv => !char.IsDigit(kv.Key[channel.Number.ToString().Length]))
                .Select(kv => kv.Value)
                .FirstOrDefault();
            channel.Icon = iconPath;
        }

        private static void ExportProgram(XmlReader rdr, ISession session)
        {
            var root = GetElement(rdr);

            var categories = root.ElementValues("category", "en");
            if (categories.Contains("Paid Programming"))
                return;

            
            var channel = BuildChannelProxy(root, session);
            var timeRange = BuildTimeRange(root);
            var title = root.ElementValue("title", "en");
            var program = new Program(channel, timeRange, title);

            program.Categories.AddAll(categories.ToList());
            program.EpisodeTitle = root.ElementValue("sub-title", "en");
            program.Description = root.ElementValue("description");
            
            var episodeIdElem = root.Elements("episode-num")
                .FirstOrDefault(e => e.Attributes("system").Any(a => a.Value == "dd_progid"));
            program.EpisodeId = episodeIdElem == null ? "" : episodeIdElem.Value;

            var episodeNumElem = root.Elements("episode-num")
                .FirstOrDefault(e => e.Attributes("system").Any(a => a.Value == "onscreen"));
            program.EpisodeNumber = episodeNumElem == null ? "" : episodeNumElem.Value;

            var credits = root.SafeElement("credits")
                .Elements()
                .Select(e => new Credit() {Role = e.Name.LocalName, Name = e.Value});
            program.Credits.AddAll(credits.ToList());

            program.Attributes.AddAll(GetAttributes(root).ToList());

            using (var tx = session.BeginTransaction())
            {
                session.Save(program);
                tx.Commit();
            }
            session.Clear();

        }

        private static TimeRange BuildTimeRange(XElement root)
        {
            var startTime = ConvertDateTime(root.AttrValue("start"));
            if (!startTime.HasValue)
                return null;
            var endTime = ConvertDateTime(root.AttrValue("stop"));
            if (!endTime.HasValue)
                return null;
            return new TimeRange(startTime.Value, endTime.Value);
        }

        private static Channel BuildChannelProxy(XElement root, ISession session)
        {
            var channelId = root.AttrValue("channel");
            return session.Load<Channel>(channelId);
        }

        private static IEnumerable<string> GetAttributes(XElement root)
        {
            var attributes = new List<string>();

            var video = root.SafeElement("video");
            var noPicture = video.SafeElement("present").Value.ToLowerInvariant() == "no";
            var blackAndWhite = video.SafeElement("colour").Value.ToLowerInvariant() == "no";
            var widescreen = video.SafeElement("aspect").Value == "16:9";
            var quality = video.ElementValue("quality");
            var audio = root.SafeElement("audio").ElementValue("stereo");

            if (noPicture)
                attributes.Add("AudioOnly");
            if (blackAndWhite)
                attributes.Add("BlackAndWhite");
            if (string.IsNullOrEmpty(quality) && widescreen)
                attributes.Add("Widescreen");
            if (!string.IsNullOrEmpty(quality))
                attributes.Add(quality);
            if (!string.IsNullOrEmpty(audio))
                attributes.Add(audio);
            return attributes;
        }

        private static XElement GetElement(XmlReader rdr)
        {
            var doc = XDocument.Parse(rdr.ReadOuterXml());
            return doc.Root;
        }

        private static DateTime? ConvertDateTime(string dateTime)
        {
            //20080715003000 -0600"
            DateTime ret;
            const string format = "yyyyMMddHHmmss zzz";
            return DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret)
                       ? (DateTime?)ret
                       : null;
        }



    }
}
