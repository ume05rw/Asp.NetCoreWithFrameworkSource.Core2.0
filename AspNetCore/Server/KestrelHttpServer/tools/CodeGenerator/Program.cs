// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace CodeGenerator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Missing path to FrameHeaders.Generated.cs");
                return 1;
            }
            else if (args.Length < 2)
            {
                Console.Error.WriteLine("Missing path to Frame.Generated.cs");
                return 1;
            }
            else if (args.Length < 3)
            {
                Console.Error.WriteLine("Missing path to HttpUtilities.Generated.cs");
                return 1;
            }

            Run(args[0], args[1], args[2]);

            return 0;
        }

        public static void Run(string knownHeadersPath, string frameFeaturesCollectionPath, string httpUtilitiesPath)
        {
            var knownHeadersContent = KnownHeaders.GeneratedFile();
            var frameFeatureCollectionContent = FrameFeatureCollection.GeneratedFile();
            var httpUtilitiesContent = HttpUtilities.HttpUtilities.GeneratedFile();

            var existingKnownHeaders = File.Exists(knownHeadersPath) ? File.ReadAllText(knownHeadersPath) : "";
            if (!string.Equals(knownHeadersContent, existingKnownHeaders))
            {
                File.WriteAllText(knownHeadersPath, knownHeadersContent);
            }

            var existingFrameFeatureCollection = File.Exists(frameFeaturesCollectionPath) ? File.ReadAllText(frameFeaturesCollectionPath) : "";
            if (!string.Equals(frameFeatureCollectionContent, existingFrameFeatureCollection))
            {
                File.WriteAllText(frameFeaturesCollectionPath, frameFeatureCollectionContent);
            }

            var existingHttpUtilities = File.Exists(httpUtilitiesPath) ? File.ReadAllText(httpUtilitiesPath) : "";
            if (!string.Equals(httpUtilitiesContent, existingHttpUtilities))
            {
                File.WriteAllText(httpUtilitiesPath, httpUtilitiesContent);
            }
        }
    }
}
