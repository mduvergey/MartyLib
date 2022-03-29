using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Marty
{
    public class FlowControlTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FlowControlTestsSimplePasses()
        {
            // Use the Assert class to test conditions
            int counter = 0;
            DoOnce doOnce = new DoOnce(() => { counter++; });
            for (int i = 0; i < 10; i++)
            {
                doOnce.Invoke();
            }
            Assert.IsTrue(counter == 1);

            DoN doN = new DoN(() => { counter++; }, 3);
            for (int i = 0; i < 10; i++)
            {
                doN.Invoke();
            }
            Assert.IsTrue(counter == 4);
        }
    }
}
