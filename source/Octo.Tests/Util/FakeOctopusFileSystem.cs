﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Octopus.Cli.Util;

namespace Octo.Tests.Util
{
    public class FakeOctopusFileSystem : IOctopusFileSystem
    {
        public Dictionary<string, string> Files { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> Deleted { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool FileExists(string path)
        {
            return Files.ContainsKey(path);
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryIsEmpty(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path, DeletionOptions options)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, DeletionOptions options)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectories(string parentDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectoriesRecursively(string parentDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string parentDirectoryPath, params string[] searchPatterns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFilesRecursively(string parentDirectoryPath, params string[] searchPatterns)
        {
            return searchPatterns
                .SelectMany(pattern => FindFilesEmulator(pattern, Files.Keys.ToArray()))
                .Distinct();
        }

        public long GetFileSize(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string path)
        {
            throw new NotImplementedException();
        }

        public void AppendToFile(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public void OverwriteFile(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string path, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string path,
            FileMode mode = FileMode.OpenOrCreate,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.Read)
        {
            throw new NotImplementedException();
        }

        public Stream CreateTemporaryFile(string extension, out string path)
        {
            throw new NotImplementedException();
        }

        public string CreateTemporaryDirectory()
        {
            throw new NotImplementedException();
        }

        public void CopyDirectory(string sourceDirectory, string targetDirectory, int overwriteFileRetryAttempts = 3)
        {
            throw new NotImplementedException();
        }

        public void CopyDirectory(string sourceDirectory,
            string targetDirectory,
            CancellationToken cancel,
            int overwriteFileRetryAttempts = 3)
        {
            throw new NotImplementedException();
        }

        public ReplaceStatus CopyFile(string sourceFile, string destinationFile, int overwriteFileRetryAttempts = 3)
        {
            throw new NotImplementedException();
        }

        public void EnsureDirectoryExists(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public string GetFullPath(string relativeOrAbsoluteFilePath)
        {
            throw new NotImplementedException();
        }

        public void OverwriteAndDelete(string originalFile, string temporaryReplacement)
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(string filePath, byte[] data)
        {
            throw new NotImplementedException();
        }

        public string RemoveInvalidFileNameChars(string path)
        {
            throw new NotImplementedException();
        }

        public void MoveFile(string sourceFile, string destinationFile)
        {
            throw new NotImplementedException();
        }

        public ReplaceStatus Replace(string path, Stream stream, int overwriteRetryAttempts = 3)
        {
            throw new NotImplementedException();
        }

        public bool EqualHash(Stream first, Stream second)
        {
            throw new NotImplementedException();
        }

        public string ReadAllText(string scriptFile)
        {
            throw new NotImplementedException();
        }

        public string[] FindFilesEmulator(string pattern, string[] names)
        {
            var matches = new List<string>();
            var regex = FindFilesPatternToRegex.Convert(pattern);
            foreach (var s in names)
            {
                if (regex.IsMatch(s))
                    matches.Add(s);
            }

            return matches.ToArray();
        }

        internal static class FindFilesPatternToRegex
        {
            static readonly Regex HasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
            static readonly Regex IllegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
            static readonly Regex CatchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
            static readonly string NonDotCharacters = @"[^.]*";

            public static Regex Convert(string pattern)
            {
                if (pattern == null)
                    throw new ArgumentNullException();
                pattern = pattern.Trim();
                if (pattern.Length == 0)
                    throw new ArgumentException("Pattern is empty.");
                if (IllegalCharactersRegex.IsMatch(pattern))
                    throw new ArgumentException("Pattern contains illegal characters.");
                var hasExtension = CatchExtentionRegex.IsMatch(pattern);
                var matchExact = false;
                if (HasQuestionMarkRegEx.IsMatch(pattern))
                    matchExact = true;
                else if (hasExtension)
                    matchExact = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
                var regexString = Regex.Escape(pattern);
                regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
                regexString = Regex.Replace(regexString, @"\\\?", ".");
                if (!matchExact && hasExtension)
                    regexString += NonDotCharacters;
                regexString += "$";
                var regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return regex;
            }
        }
    }
}
