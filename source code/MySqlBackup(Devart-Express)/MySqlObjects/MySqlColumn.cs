using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Devart.Data.MySql
{
    public class MySqlColumn
    {
        public enum DataWrapper
        {
            None,
            Sql
        }

        string _name = "";
        Type _dataType = typeof(string);
        string _mySqlDataType = "";
        string _collation = "";
        bool _allowNull = true;
        string _key = "";
        string _defaultValue = "";
        string _extra = "";
        string _privileges = "";
        string _comment = "";
        bool _isPrimaryKey = false;
        int _timeFractionLength = 0;
        bool _isGeneratedColumn = false;

        public string Name { get { return _name; } }
        public Type DataType { get { return _dataType; } }
        public string MySqlDataType { get { return _mySqlDataType; } }
        public string Collation { get { return _collation; } }
        public bool AllowNull { get { return _allowNull; } }
        public string Key { get { return _key; } }
        public string DefaultValue { get { return _defaultValue; } }
        public string Extra { get { return _extra; } }
        public string Privileges { get { return _privileges; } }
        public string Comment { get { return _comment; } }
        public bool IsPrimaryKey { get { return _isPrimaryKey; } }
        public int TimeFractionLength { get { return _timeFractionLength; } }
        public bool IsGeneratedColumn { get { return _isGeneratedColumn; } }

        public MySqlColumn(string name, Type type, string mySqlDataType,
            string collation, bool allowNull, string key, string defaultValue,
            string extra, string privileges, string comment)
        {
            _name = name;
            _dataType = type;
            _mySqlDataType = mySqlDataType.ToLower();
            _collation = collation;
            _allowNull = allowNull;
            _key = key;
            _defaultValue = defaultValue;
            _extra = extra;
            _privileges = privileges;
            _comment = comment;

            if (key.ToLower() == "pri")
            {
                _isPrimaryKey = true;
            }

            if (_dataType == typeof(DateTime))
            {
                if (_mySqlDataType.Length > 8)
                {
                    string _fractionLength = "";
                    foreach (var __dL in _mySqlDataType)
                    {
                        if (Char.IsNumber(__dL))
                            _fractionLength += Convert.ToString(__dL);
                    }

                    if (_fractionLength.Length > 0)
                    {
                        _timeFractionLength = 0;
                        int.TryParse(_fractionLength, out _timeFractionLength);
                    }
                }
            }

            if (_extra.ToLower().Contains("generated"))
            {
                _isGeneratedColumn = true;
            }
            else
            {
                _isGeneratedColumn = false;
            }
        }
    }
}