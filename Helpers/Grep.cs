using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IncidentAssistant.Core
{
	/// <summary>
	/// Basic Linux Grepping implementation for Windows/.NET
	/// </summary>
	public class FSGrep
	{
		public FSGrep() => Recursive = true;
		public string RootPath { get; set; } = string.Empty;
		public bool Recursive { get; set; }
		public string FileSearchMask { get; set; } = string.Empty;
		public string FileSearchLinePattern { get; set; } = string.Empty;
		public string FileNamePattern { get; set; } = string.Empty;

		public IEnumerable<string> GetFileNames()
		{
			if (!Directory.Exists(RootPath))
				throw new ArgumentException("GetFileNames() -- Can't find RootPath!");

			if (string.IsNullOrWhiteSpace(FileSearchMask))
				throw new ArgumentException("GetFileNames() -- FileSearchPattern is empty; use *.*!");

			SearchOption searchOptions = Recursive
				? SearchOption.AllDirectories
				: SearchOption.TopDirectoryOnly;

			if (FileSearchMask.Contains(','))
			{
				string[] masks = FileSearchMask.Split(',');
				IEnumerable<string> results = Directory.EnumerateFiles(RootPath, masks[0], searchOptions)
					.Where(path => Regex.IsMatch(path, FileNamePattern));

				if (masks.Length > 1)
				{
					for (int index = 1; index < masks.Length; index++)
					{
						results = results.Concat(Directory.EnumerateFiles(RootPath, masks[index], searchOptions));
					}
				}

				return results;
			}

			return Directory.EnumerateFiles(RootPath, FileSearchMask, searchOptions)
				.Where(path => Regex.IsMatch(path, FileNamePattern));
		}

		public IEnumerable<GrepResult> GetMatchingFiles()
		{
			int lineNumber = 0;
			foreach (string filePath in GetFileNames())
			{
				foreach (string line in File.ReadAllLines(filePath))
				{
					if (System.Text.RegularExpressions.Regex.Match(line, FileSearchLinePattern).Success)
						yield return new GrepResult()
						{
							FilePath = filePath,
							FileName = Path.GetFileName(filePath),
							LineNumber = lineNumber,
							Line = line
						};

					lineNumber++;
				}
			}
		}

		public class GrepResult
		{
			public string FilePath { get; set; }
			public string FileName { get; set; }
			public string Line { get; set; }
			public int LineNumber { get; set; }

			public override string ToString() => $"--file {FilePath}:{LineNumber}";
		}
	}
}
