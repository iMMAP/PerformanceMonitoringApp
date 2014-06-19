using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class Organization
    {
        private int _id;      
        private string _name;       
        private string _acronym;
        private int _typeId;       
        private int _countryId;
        private bool _status;
        private string _phone;       
        private string _address;
        private Guid _userId;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }    
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Acronym
        {
            get { return _acronym; }
            set { _acronym = value; }
        }
        public int CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }
        public int TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }
        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public Organization()
        {
            this._name = string.Empty;
            this._acronym = string.Empty;
            this._typeId = -1;
            this._countryId = -1;
            this._status = true;
            this._phone = string.Empty;
            this._address = string.Empty;
        }

        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public DataTable GetOrganizations(object[] parameters)
        {           
            return DBContext.GetData("GetOrganizationsWithFilters", parameters);
        }

        public Organization GetOrganizationByID(int ID)
        {
            object[] parameters = new object[] { ID, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
            DataTable dt = DBContext.GetData("GetOrganizationsWithFilters", parameters);
            Organization obj =null;
            if (dt != null && dt.Rows.Count > 0)
            {         
                obj = new Organization();
                obj.Id = Convert.ToInt32(dt.Rows[0]["OrganizationId"]);
                obj.Name = dt.Rows[0]["OrganizationName"].ToString();
                obj.Acronym = dt.Rows[0]["OrganizationAcronym"].ToString();
                obj.TypeId = Convert.ToInt32(dt.Rows[0]["OrganizationTypeId"]);
                obj.Status = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                obj.Phone = dt.Rows[0]["Phone"].ToString();
                obj.Address = dt.Rows[0]["Address"].ToString();
                obj.CountryId = Convert.ToInt32(dt.Rows[0]["CountryId"]);
            }
            return obj;
        }

        public int Add()
        {
            string organizationName = this._name;
            string organizationAcronym = this._acronym;
            int typeId = this._typeId;
            int countryId = this._countryId;
            bool status = this._status;
            string phone = string.IsNullOrEmpty(this._phone) ? null:this._phone;
            string address = string.IsNullOrEmpty(this._address) ? null: this._address;
            Guid userId = this._userId;
            object[] parameters = new object[] { organizationName, organizationAcronym, typeId, countryId, status, phone, address, userId, DBNull.Value };
            return DBContext.Add("InsertOrganization", parameters);

        }

        public int Update()
        {
            string organizationName = this._name;
            string organizationAcronym = this._acronym;
            int typeId = this._typeId;
            int countryId = this._countryId;
            bool status = this._status;
            string phone = string.IsNullOrEmpty(this._phone) ? null : this._phone;
            string address = string.IsNullOrEmpty(this._address) ? null : this._address;
            int id = this._id;
            Guid userId = this._userId;
            object[] parameters = new object[] { id,organizationName, organizationAcronym, typeId, countryId, status, phone, address, userId, DBNull.Value };
            return DBContext.Update("UpdateOrganization", parameters);

        }

        public void Delete(int orgId)
        {
            DBContext.Delete("DeleteOrganization", new object[] { orgId, DBNull.Value });
        }
        public bool AnyUserExistsInOrganization(int orgId)
        {
            DataTable dt = DBContext.GetData("IsOrganizationHasUsers", new object[] { orgId });
            return dt.Rows.Count > 0;
        }
        public DataTable GetOrganizationTypes()
        {
            return DBContext.GetData("GetOrganizationTypes");
        }

    }
}
