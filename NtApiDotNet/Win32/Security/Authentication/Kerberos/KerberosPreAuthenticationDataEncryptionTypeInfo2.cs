﻿//  Copyright 2022 Google LLC. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using NtApiDotNet.Utilities.ASN1;
using NtApiDotNet.Utilities.ASN1.Builder;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NtApiDotNet.Win32.Security.Authentication.Kerberos
{
    /// <summary>
    /// Class to represent a PA-ETYPE-INFO2 structure.
    /// </summary>
    public sealed class KerberosPreAuthenticationDataEncryptionTypeInfo2 : KerberosPreAuthenticationData
    {
        /// <summary>
        /// The list of encryption info entries.
        /// </summary>
        public IReadOnlyList<KerberosEncryptionTypeInfo2Entry> Entries { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entries">The list of encryption type entries.</param>
        public KerberosPreAuthenticationDataEncryptionTypeInfo2(IEnumerable<KerberosEncryptionTypeInfo2Entry> entries)
            : base(KerberosPreAuthenticationType.PA_ETYPE_INFO2)
        {
            Entries = entries.ToList().AsReadOnly();
        }

        internal static KerberosPreAuthenticationDataEncryptionTypeInfo2 Parse(byte[] data)
        {
            DERValue[] values = DERParser.ParseData(data);
            if (!values.CheckValueSequence())
                throw new InvalidDataException();

            return new KerberosPreAuthenticationDataEncryptionTypeInfo2(values[0].Children.Select(KerberosEncryptionTypeInfo2Entry.Parse));
        }

        private protected override byte[] GetData()
        {
            DERBuilder builder = new DERBuilder();
            builder.WriteSequence(Entries);
            return builder.ToArray();
        }
    }
}
