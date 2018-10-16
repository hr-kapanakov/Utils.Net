using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Controls;
using Utils.Net.Interfaces;
using Utils.NetTests;

namespace Utils.Net.Managers.Tests
{
    [TestClass]
    public class TutorialManagerTests
    {
        private ITutorialManager tutorialManager;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() =>
            {
                tutorialManager = ((Sample.MainWindowViewModel)UITester.MainWindow.DataContext).TutorialManager;
                tutorialManager.Start();
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            UITester.Dispatcher.Invoke(() => tutorialManager.Stop());
        }


        [TestMethod]
        public void ItemIdTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var tutorialButton = UITester.Get<Button>(b => b.ToolTip.ToString() == "Tutorial");
                TutorialManager.SetItemId(tutorialButton, "test");
                Assert.AreEqual(TutorialManager.GetItemId(tutorialButton), "test");
                TutorialManager.SetItemId(tutorialButton, null);
            });
        }

        [TestMethod]
        public void DontShowAgainTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var value = tutorialManager.DontShowAgain;
                tutorialManager.DontShowAgain = !value;
                tutorialManager.DontShowAgain = !value;
                Assert.AreEqual(tutorialManager.DontShowAgain, !value);

                var changed = false;
                tutorialManager.DontShowAgainChanged += (_, __) => changed = true;

                tutorialManager.DontShowAgain = value;
                Assert.AreEqual(tutorialManager.DontShowAgain, value);

                Assert.IsTrue(changed);
            });
        }

        [TestMethod]
        public void StartTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                tutorialManager.Stop();
                Assert.IsFalse(tutorialManager.IsStarted);

                bool started = false;
                tutorialManager.Started += (_, __) => started = true;
                bool currentChanged = false;
                tutorialManager.CurrentItemChanged += (_, __) => currentChanged = true;

                tutorialManager.Start();

                Assert.IsTrue(started);
                Assert.IsTrue(currentChanged);
                Assert.IsTrue(tutorialManager.IsStarted);
                Assert.AreEqual(tutorialManager.CurrentItemId, tutorialManager.Items.Keys.First());
                Assert.AreEqual(tutorialManager.CurrentItem, tutorialManager.Items.Values.First());

                // for code coverage
                var manager = new TutorialManager();
                manager.Start();
            });
        }

        [TestMethod]
        public void PreviousTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                tutorialManager.Next();
                tutorialManager.Next();
                tutorialManager.Previous();

                bool currentChanged = false;
                tutorialManager.CurrentItemChanged += (_, __) => currentChanged = true;

                tutorialManager.Previous();

                Assert.IsTrue(currentChanged);
                Assert.IsTrue(tutorialManager.IsStarted);
                Assert.AreEqual(tutorialManager.CurrentItemId, tutorialManager.Items.Keys.First());
                Assert.AreEqual(tutorialManager.CurrentItem, tutorialManager.Items.Values.First());

                tutorialManager.Previous();
                Assert.IsFalse(tutorialManager.IsStarted);

                // for code coverage
                var manager = new TutorialManager();
                manager.Previous();
            });
        }

        [TestMethod]
        [Timeout(10000)]
        public void NextTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                tutorialManager.Next();

                bool currentChanged = false;
                tutorialManager.CurrentItemChanged += (_, __) => currentChanged = true;

                tutorialManager.Next();

                Assert.IsTrue(currentChanged);
                Assert.IsTrue(tutorialManager.IsStarted);
                Assert.AreEqual(tutorialManager.CurrentItemId, tutorialManager.Items.Keys.ElementAt(2));
                Assert.AreEqual(tutorialManager.CurrentItem, tutorialManager.Items.Values.ElementAt(2));

                while (tutorialManager.IsStarted)
                {
                    tutorialManager.Next();
                }

                // for code coverage
                var manager = new TutorialManager();
                manager.Next();
            });
        }

        [TestMethod]
        public void StopTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool stopped = false;
                tutorialManager.Stopped += (_, __) => stopped = true;

                tutorialManager.Stop();
                Assert.IsTrue(stopped);
                Assert.IsFalse(tutorialManager.IsStarted);
                Assert.IsNull(tutorialManager.CurrentItemId);
                Assert.IsNull(tutorialManager.CurrentItem);
            });
        }

        [TestMethod]
        public void DisposeTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                tutorialManager.Dispose();
                Assert.IsFalse(tutorialManager.IsStarted);
            });
        }

        [TestMethod]
        public void OwnerActivateTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                UITester.MainWindow.Activate();
            });
        }

        [TestMethod]
        public void OwnerMoveTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                UITester.MainWindow.Left += 10;

                tutorialManager.Next();
                tutorialManager.Next();
                tutorialManager.Next();
                UITester.MainWindow.Left += 10;
            });
        }
    }
}
