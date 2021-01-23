using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubApiUtils.Test
{
    [TestClass]
    public class TestGitHubApiServer
    {
        [TestMethod]
        public async Task TestGetRepoReleaseListNotNullForValidRepo()
        {
            var api = new GitHubApiServer(new HttpClient());
            var releases = await api.GetRepoReleaseList("sobu86", "GitHubApiUtils");

            Assert.IsNotNull(releases);

            Console.WriteLine($"GitHubApiUtils Release Count - {releases.Count}");
            releases.ForEach(r => Console.WriteLine($"Release published at - {r.PublishedAt}"));
        }

        [TestMethod]
        public async Task TestGetLastestRepoReleaseIsNullForNoReleases()
        {
            var api = new GitHubApiServer(new HttpClient());
            var latest = await api.GetLatestRepoRelease("sobu86", "sobu86.github.io");

            // No releases are planned for sobu86.github.io
            Assert.IsNull(latest);
        }

        [TestMethod]
        public async Task TestGetRepoReleaseListContainsElements()
        {
            var api = new GitHubApiServer(new HttpClient());
            var releases = await api.GetRepoReleaseList("sobu86", "TempRepoForGitHubApiTest");

            Assert.IsNotNull(releases);
            Assert.IsTrue(releases.Count > 0);

            Console.WriteLine($"GitHubApiUtils Release Count - {releases.Count}");
            releases.ForEach(r => Console.WriteLine($"Release published at - {r.PublishedAt}"));
        }

        [TestMethod]
        public async Task TestGetLastestRepoReleaseIsNotNull()
        {
            var api = new GitHubApiServer(new HttpClient());
            var latest = await api.GetLatestRepoRelease("sobu86", "TempRepoForGitHubApiTest");

            // No releases are planned for sobu86.github.io
            Assert.IsNotNull(latest);
        }

        [TestMethod]
        public async Task TestGetLastestRepoReleaseIsActuallyLatestInList()
        {
            var api = new GitHubApiServer(new HttpClient());
            var latest = await api.GetLatestRepoRelease("sobu86", "TempRepoForGitHubApiTest");
            var releases = await api.GetRepoReleaseList("sobu86", "TempRepoForGitHubApiTest");

            DateTime latestInList = DateTime.MinValue;
            releases.ForEach(r => latestInList =
                    (r.PublishedAt > latestInList) ? r.PublishedAt : latestInList);

            Assert.AreEqual(latestInList, latest.PublishedAt);

            Console.WriteLine($"latest release @ {latest.PublishedAt}");
        }

        [TestMethod]
        public async Task TestInvalidRepoNameReleaseListThrowsException()
        {
            var api = new GitHubApiServer(new HttpClient());

            bool exception = false;

            try
            {
                await api.GetRepoReleaseList("sobu86", "invalid_repo");
            }
            catch (Exception)
            {
                exception = true;
            }

            Assert.IsTrue(exception);
        }

        [TestMethod]
        public async Task TestInvalidRepoNameLatestReleaseThrowsException()
        {
            var api = new GitHubApiServer(new HttpClient());

            bool exception = false;

            try
            {
                await api.GetLatestRepoRelease("sobu86", "invalid_repo");
            }
            catch (Exception)
            {
                exception = true;
            }

            Assert.IsTrue(exception);
        }
    }
}
