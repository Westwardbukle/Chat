using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Chat.Core.Abstract;

namespace Chat.Core.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            
            byte[] dst = new byte[0x31];
            
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            
            return Convert.ToBase64String(dst);
        }
        
        public bool VerifyHashedPassword(string password, string passwordHash)
        {
            byte[] buffer4;
            
            if (passwordHash == null)
            {
                return false;
            }
            
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            
            byte[] src = Convert.FromBase64String(passwordHash);
            
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            
            return ByteArraysEqual(buffer3, buffer4);
        }
        
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Count != b.Count)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Count; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}