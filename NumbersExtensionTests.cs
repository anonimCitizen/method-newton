using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using static MethodNewtonTask.NumbersExtension;

#pragma warning disable CA1707

namespace MethodNewtonTask.Tests
{
    [TestFixture]
    public class NumbersExtensionTests
    {
        private static IConfigurationRoot ConfigurationRoot { get; } =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        [OneTimeSetUp]
        public void SetUp()
        {
            try
            {
                NumbersExtension.AppSettings.Epsilon = double.Parse(ConfigurationRoot["Epsilon"]);
            }
            catch
            {
                NumbersExtension.AppSettings.Epsilon = 0.1;
            }
        }

        [TestCase(1, 5, 0.0001, 1)]
        [TestCase(8, 3, 0.0001, 2)]
        [TestCase(0.001, 3, 0.0001, 0.1)]
        [TestCase(0.04100625, 4, 0.0001, 0.45)]
        [TestCase(8, 3, 0.000001, 2)]
        [TestCase(0.0279936, 7, 0.0001, 0.6)]
        [TestCase(0.0081, 4, 0.01, 0.3)]
        [TestCase(-0.008, 3, 0.01, -0.2)]
        [TestCase(0.004241979, 9, 0.00000001, 0.545)]
        [TestCase(100, 2, 0.0001, 10)]
        [TestCase(-100, 3, 0.0001, -4.6416)]
        [TestCase(12345678, 5, 0.0001, 26.2001)]
        [TestCase(9876543210, 9, 0.0001, 12.8977)]
        public void FindNthRootTests(double number, int n, double accuracy, double expected)
        {
            Assert.AreEqual(expected, FindNthRoot(number, n, accuracy), accuracy);
        }

        [Test]
        public void FindNthRoot_NumberIsNegativeAndDegreeIsEven_ThrowArgumentException() =>
            Assert.Throws<ArgumentException>(() => FindNthRoot(-0.01, 2, 0.0001), "A cannot be negative when when the n degree is even.");

        [Test]
        public void FindNthRoot_DegreeIsNegative_ThrowArgumentException() =>
            Assert.Throws<ArgumentException>(() => FindNthRoot(0.001, -2, 0.0001), "Degree can not be less or equal zero.");

        [Test]
        public void FindNthRoot_AccuracyIsLessThanZero_ThrowArgumentOutOfRangeException() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => FindNthRoot(0.01, 2, -1), "Accuracy cannot be less than zero.");

        [Test]
        public void FindNthRoot_AccuracyIsMoreThanEpsilon_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => FindNthRoot(0.01, 2, NumbersExtension.AppSettings.Epsilon + 0.1), $"Accuracy should be less than {NumbersExtension.AppSettings.Epsilon}");
        }

        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NaN)]
        public void FindNthRoot_NumberIsNotAFiniteValue_ThrowArgumentException(double number) =>
            Assert.Throws<ArgumentException>(() => FindNthRoot(number, 2, 0.0001), $"{nameof(number)} is not a finite value");
    }
}