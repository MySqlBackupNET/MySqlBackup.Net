using System;

namespace MySqlConnector
{
    public class MySqlColumn
    {
        public enum DataWrapper
        {
            None,
            Sql
        }

        string _name = string.Empty;
        Type _dataType = typeof(string);
        string _mySqlDataType = string.Empty;
        string _collation = string.Empty;
        bool _allowNull = true;
        string _key = string.Empty;
        string _defaultValue = string.Empty;
        string _extra = string.Empty;
        string _privileges = string.Empty;
        string _comment = string.Empty;
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

            if (_dataType == typeof(DateTime) || _dataType == typeof(TimeSpan) ||
                _mySqlDataType.StartsWith("time") || _mySqlDataType.StartsWith("datetime") || _mySqlDataType.StartsWith("timestamp"))
            {
                if (_mySqlDataType.Length > 4) // Changed from 8 to 4 to catch "time(6)"
                {
                    string _fractionLength = string.Empty;
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

            if (_extra.ToUpper() == "VIRTUAL GENERATED" || _extra.ToUpper() == "STORED GENERATED")
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