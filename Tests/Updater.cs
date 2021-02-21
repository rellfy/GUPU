using System;
using NUnit.Framework;
using UnityEngine;

namespace GUPU.Tests {
    public class Updater {
        [Test]
        public void GetLastCommitHash_ValidRepoUrl_DoesNotThrow() {
            string hash = GUPU.Updater.GetLastCommitHash(
                "https://github.com/rellfy/GUPU.git"
            );
            Assert.Equals(hash.Length, 40);
        }
        
        [Test]
        public void GetLastCommitHash_InvalidRepoUrl_Throws() {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                GUPU.Updater.GetLastCommitHash(
                    "https://github.com/rellfy/fakeGUPU.git"
                );
            });
        }
    }
}