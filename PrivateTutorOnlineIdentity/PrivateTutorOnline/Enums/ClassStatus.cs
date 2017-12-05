using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Enums
{
    public enum ClassStatus
    {
        ClassClose, WaitingForAdminApproval, AdminApproved, AdminReject, WaitingForCustomerApproval, CustomerApproved, CustomerRejected
    }
}