using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientRoomManagement.Utilities
{
    public class IllegalAssignmentException : Exception
    {
        public IllegalAssignmentException()
        {
        }

        public IllegalAssignmentException(string message) : base(message)
        {
        }

        public IllegalAssignmentException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}