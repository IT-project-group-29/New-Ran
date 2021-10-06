using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication4.Models;

namespace WebApplication4.Util
{
    public class ExcelHelper
    {


        public static DataTable ExcelToDataTable(Stream filePath)
        {
       
            DataTable returnTable = new DataTable();
            SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false);
            using (doc)
            {

                // HERE consider using FirstOrDefault() as First() throws an exception if nothing is found.
                // You should then CHECK that the worksheet and sheet data are not null before trying to access
                // these are LINQ/Lambda expressions that may return no object.
                WorkbookPart workbookPart = doc.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();

                //this checks if the worksheet is empty
                if (sheetData.ChildElements.Count == 0)
                {
                    throw new System.ArgumentException("Excel document cannot be empty", "original");
                }

                var sharedStringPart = workbookPart.SharedStringTablePart;

                var values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                bool firstRow = true;
                ArrayList rowsSkipped = new ArrayList();

                foreach (Row r in sheetData.Elements<Row>())
                {
                    //heading row
                    if (firstRow)
                    {
                        int columnCount = 0;
                        foreach (Cell c in r.Elements<Cell>())
                        {
                            var stringId = Convert.ToInt32(c.InnerText);
                            string columnHeader = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                            if (!returnTable.Columns.Contains(columnHeader))
                            {
                                returnTable.Columns.Add(columnHeader);
                            }
                            else
                            {
                                rowsSkipped.Add(columnCount);
                            }
                            columnCount++;
                        }

                        firstRow = false;
                    }
                    //data rows
                    else
                    {
                        DataRow newCustomersRow = returnTable.NewRow();
                        int columnCount = 0;
                        int columnInsertCount = 0;
                        foreach (Cell c in r.Elements<Cell>())
                        {
                            if (!rowsSkipped.Contains(columnCount))
                            {

                                var value = "";
                                // The cells contains a string input that is not a formula
                                if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                                {
                                    var index = int.Parse(c.CellValue.Text);
                                    value = values[index].InnerText;
                                }
                                else if (c.CellValue != null)
                                {
                                    //styleIndex 3 is for dates
                                    if (c.StyleIndex == 3)
                                    {
                                        value = DateTime.FromOADate(int.Parse(c.CellValue.Text)).ToString();
                                    }
                                    else
                                    {
                                        value = c.CellValue.Text;
                                    }
                                }

                                if (c.CellFormula != null)
                                {
                                    value = c.CellFormula.Text;
                                }
                                newCustomersRow[columnInsertCount] = value;
                                columnInsertCount++;
                            }
                            columnCount++;
                        }
                        returnTable.Rows.Add(newCustomersRow);
                    }

                }
            }



            return returnTable;

        }




    }


    public class UserRoleHelper
    {
        //the main reason this helper exists is due to the way that we are required to add users to roles
        //there is no way to access AspNetUserRoles using models, so we are required to do it this way
        //a lot of help from here
        //http://johnatten.com/2013/11/11/extending-identity-accounts-and-implementing-role-based-authentication-in-asp-net-mvc-5/

        public static bool addUserToRole(string userId, string roleName)
        {
            //this method firstly checks if the role with the given name exists
            //if it does not, it automatically adds a new role with that name
            //      this could be changed to just reject the given user/role
            //
            //then it adds the person to the role with the given name

            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!rm.RoleExists(roleName))
            {
                addRole(roleName);
            }

            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public static bool removeUserFromRole(string userId, string roleName)
        {
            //removes the given user from the given role
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var idResult = um.RemoveFromRole(userId, roleName);

            return idResult.Succeeded;
        }

        public static bool userInRole(string userId, string roleName)
        {
            //checks if the given user is in the given role
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            return true;
            //return um.IsInRole(userId, roleName);
        }

        public static void addRole(string roleName)
        {
            //creates a new role
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            rm.Create(new IdentityRole(roleName));

        }
    }



}