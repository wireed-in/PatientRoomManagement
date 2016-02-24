using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientRoomManagement.Models;

namespace PatientRoomManagement.Tests.Features
{
    [TestClass]
    public class AssignmentFeatures
    {
        [TestMethod]
        public void TestSettingRoomGenderUponAssignment()
        {
            // Arrange
            // Create patient and room instances
            var patient = new Patient()
            {
                Id = 13,
                FirstName = "Test",
                LastName = "Tester",
                Gender = "Male",
                Dob = Convert.ToDateTime("2000/12/12"),
                Mrn = "001-12345"
            };

            var room = new Room()
            {
                Id = 13,
                Number = 105,
                NumberOfBeds = 1,
                Gender = null,
                Assignments = new List<Assignment>()
            };

            // Act
            // Assign patient to a room
            room.Assignments.Add(Assignment.Create(patient, room));

            // Assert
            // room gender should adopt the patien's gender
            Assert.AreEqual(patient.Gender, room.Gender);
        }

        [TestMethod]
        public void TestingRoomAvailabilityBeforeAnyAssignments()
        {
            // Arrange
            // Create a room instance with empty assignments
            var room = new Room()
            {
                Id = 13,
                Number = 105,
                NumberOfBeds = 1,
                Gender = null,
                Assignments = new List<Assignment>()
            };

            // Act - Nothing to act on here

            // Assert
            Assert.AreEqual(room.NumberOfBeds, room.AvailableSpace);
        }

        [TestMethod]
        public void TestingRoomAvailabilityWhenItsFull()
        {
            // Arrange
            // Create patient and room instances
            var patient = new Patient()
            {
                Id = 13,
                FirstName = "Test",
                LastName = "Tester",
                Gender = "Male",
                Dob = Convert.ToDateTime("2000/12/12"),
                Mrn = "001-12345"
            };

            var room = new Room()
            {
                Id = 13,
                Number = 105,
                NumberOfBeds = 1,
                Gender = null,
                Assignments = new List<Assignment>()
            };

            // Act
            // Assign patient to a room
            room.Assignments.Add(Assignment.Create(patient, room));

            // Assert
            Assert.AreEqual(0, room.AvailableSpace);
        }
    }
}
