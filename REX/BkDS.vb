Partial Class BkDS
    Partial Public Class Cash_Register1DataTable


    End Class

    Partial Public Class OrderDetailedDataTable
    End Class

    Public Enum SearchBookingBy
        BkNo
        Branch
        Branch_Name
        Veh_Name
        Veh_Model
        Veh_Model_Id
        Cust_Name
        Sc_Id
        Hypt_To
        Sc_Id_Asm
        Team_LeaderId
        Sc_Name
        Cancelled
        Invoice_Cancelled
        Cust_Tel_Mob
        DeliveredOK
        Prospect_No
        Invoiced
        InvoiceNo
        Chassis_No
        PostedToTally
        Engin_No

    End Enum


    Public Enum searchEntry_TypeBy
        Receipt
        Payment
    End Enum

    Public Enum searchEntryHeadBy
        Entry_Head
        Entry_sub_head
        Entry_Type
    End Enum

    Public Enum searchcashRegisterBy
        Entry_No
        Entry_Type
        Entry_Date
        Amount
        Ref_no
        Entry_Head
        Entry_sub_head
        Party
        Branch
        Payment_Mode
        Receipt
        User_Id
        JobCardNo
    End Enum

    Public Enum SearchVehicleBy
        Vehicle_Model
        Vehicle_Name
        Vehicle_Color
        Vehicle_Model_Code
        Chs_No
        Eng_No
        IsLocal
        V_Status
        InvNo
        Inv_Error
        Mfd_Year
        Veh_Fuel
        Product_Name
        Del_ChalanNo
        FromLoc_Name
        ToLoc_Name
        Veh_TypeName
        PostedToTally
        BranchId
    End Enum

End Class
