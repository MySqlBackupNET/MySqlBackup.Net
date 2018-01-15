using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql.Data
{
    public class Resources
    {
        public const string BadVersionFormat = "Version string not in acceptable format";
        public const string NamedPipeNoSeek = "NamedPipeStream does not support seeking";
        public const string StreamAlreadyClosed = "The stream has already been closed";
        public const string BufferCannotBeNull = " The buffer cannot be null";
        public const string BufferNotLargeEnough = " Buffer is not large enough";
        public const string OffsetCannotBeNegative = " Offset cannot be negative";
        public const string CountCannotBeNegative = " Count cannot be negative";
        public const string StreamNoRead = " The stream does not support reading";
        public const string NamedPipeNoSetLength = "NamedPipeStream doesn't support SetLength";
        public const string StreamNoWrite = "The stream does not support writing";
        public const string ErrorCreatingSocket = "Error creating socket connection";
        public const string SocketNoSeek = "Socket streams do not support seeking";
        public const string UnixSocketsNotSupported = "Unix sockets are not supported on Windows";
        public const string OffsetMustBeValid = "Offset must be a valid position in buffer";
        public const string CSNoSetLength = "SetLength is not a valid operation on CompressedStream";
        public const string FromIndexMustBeValid = "From index must be a valid index inside the from buffer";
        public const string FromAndLengthTooBig = "From index and length use more bytes than from contains";
        public const string IndexMustBeValid = "Index must be a valid position in the buffer";
        public const string IndexAndLengthTooBig = "Index and length use more bytes than to has room for";
        public const string PasswordMustHaveLegalChars = "Password must be valid and contain length characters";
        public const string ParameterCannotBeNegative = "Parameter cannot have a negative value";
        public const string ConnectionMustBeOpen = "Connection must be valid and open";
        public const string DataReaderOpen = "There is already an open DataReader associated with this Connection which must be closed first.";
        public const string SPNotSupported = "Stored procedures are not supported on this version of MySQL";
        public const string ConnectionNotSet = "The connection property has not been set or is null.";
        public const string ConnectionNotOpen = "The connection is not open.";
        public const string AdapterIsNull = "Improper MySqlCommandBuilder state: adapter is null";
        public const string AdapterSelectIsNull = "Improper MySqlCommandBuilder state: adapter's SelectCommand is null";
        public const string CBMultiTableNotSupported = "MySqlCommandBuilder does not support multi-table statements";
        public const string CBNoKeyColumn = "MySqlCommandBuilder cannot operate on tables with no unique or key columns";
        public const string ParameterCannotBeNull = "Parameter cannot be null";
        public const string ChaosNotSupported = "Chaos isolation level is not supported ";
        public const string ParameterIsInvalid = "Parameter is invalid.";
        public const string ConnectionAlreadyOpen = "The connection is already open.";
        public const string KeywordNotSupported = "Option not supported.";
        public const string WriteToStreamFailed = "Writing to the stream failed.";
        public const string ReadFromStreamFailed = "Reading from the stream has failed.";
        public const string QueryTooLarge = "Packets larger than max_allowed_packet are not allowed.";
        public const string UnableToExecuteSP = "Unable to execute stored procedure '{0}'.";
        public const string ProcAndFuncSameName = "same name are not supported.";
        public const string KeywordNoNull = "Keyword does not allow null values.";
        public const string ImproperValueFormat = "Value has an unsupported format.";
        public const string InvalidProcName = "Procedure or function '{0}' cannot be found in database '{1}'.";
        public const string HardProcQuery = "Retrieving procedure metadata for {0} from server.";
        public const string SoftProcQuery = "Retrieving procedure metadata for {0} from procedure cache.";
        public const string ConnectionBroken = "Connection unexpectedly terminated.";
        public const string IncorrectTransmission = "An incorrect response was received from the server.";
        public const string CancelNotSupported = "Canceling an active query is only supported on MySQL 5.0.0 and above. ";
        public const string Timeout = "Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.";
        public const string CancelNeeds50 = "Canceling an executing query requires MySQL 5.0 or higher.";
        public const string NoNestedTransactions = "Nested transactions are not supported.";
        public const string CommandTextNotInitialized = "The CommandText property has not been properly initialized.";
        public const string UnableToParseFK = "There was an error parsing the foreign key definition.";
        public const string PerfMonCategoryHelp = "This category includes a series of counters for MySQL";
        public const string PerfMonCategoryName = ".NET Data Provider for MySQL";
        public const string PerfMonHardProcHelp = "The number of times a procedures metadata had to be queried from the server.";
        public const string PerfMonHardProcName = "Hard Procedure Queries";
        public const string PerfMonSoftProcHelp = "The number of times a procedures metadata was retrieved from the client-side cache.";
        public const string PerfMonSoftProcName = "Soft Procedure Queries";
        public const string WrongParameterName = "Parameter '{0}' is not found but a parameter with the name '{1}' is found. Parameter names must include the leading parameter marker.";
        public const string UnableToConnectToHost = "Unable to connect to any of the specified MySQL hosts.";
        public const string UnableToRetrieveParameters = "Unable to retrieve stored procedure metadata for routine '{0}'.  Either grant  SELECT privilege to mysql.proc for this user or use \"check parameters=false\" with  your connection string.";
        public const string NextResultIsClosed = "Invalid attempt to call NextResult when the reader is closed.";
        public const string NoBodiesAndTypeNotSet = "When calling stored procedures and 'Use Procedure Bodies' is false, all parameters must have their type explicitly set.";
        public const string TimeoutGettingConnection = "error connecting: Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached.";
        public const string ParameterAlreadyDefined = "Parameter '{0}' has already been defined.";
        public const string ParameterMustBeDefined = "Parameter '{0}' must be defined.";
        public const string ObjectDisposed = "The object is not open or has been disposed.";
        public const string MultipleConnectionsInTransactionNotSupported = "Multiple simultaneous connections or connections with different connection strings inside the same transaction are not currently supported.";
        public const string DistributedTxnNotSupported = "MySQL Connector/Net does not currently support distributed transactions.";
        public const string FatalErrorDuringExecute = "Fatal error encountered during command execution.";
        public const string FatalErrorDuringRead = "Fatal error encountered during data read.";
        public const string FatalErrorReadingResult = "Fatal error encountered attempting to read the resultset.";
        public const string RoutineNotFound = "Routine '{0}' cannot be found. Either check the spelling or make sure you have sufficient rights to execute the routine.";
        public const string ParameterNotFoundDuringPrepare = "Parameter '{0}' was not found during prepare.";
        public const string ValueNotSupportedForGuid = "The requested column value could not be treated as or conveted to a Guid.";
        public const string UnableToDeriveParameters = "Unable to derive stored routine parameters.  The 'Parameters' information schema table is not available and access to the stored procedure body has been disabled.";
        public const string DefaultEncodingNotFound = "The default connection encoding was not found. Please report this as a bug along with your connection string and system details.";
        public const string GetHostEntryFailed = "Call to GetHostEntry failed after {0} while querying for hostname '{1}': SocketErrorCode={2}, ErrorCode={3}, NativeErrorCode={4}.";
        public const string UnableToEnumerateUDF = "An error occured attempting to enumerate the user-defined functions.  Do you have SELECT privileges on the mysql.func table?";
        public const string DataNotInSupportedFormat = "The given value was not in a supported format.";
        public const string NoServerSSLSupport = "The host {0} does not support SSL connections.";
        public const string CouldNotFindColumnName = "Could not find specified column in results: {0}";
        public const string InvalidColumnOrdinal = "You have specified an invalid column ordinal.";
        public const string ReadingPriorColumnUsingSeqAccess = "Invalid attempt to read a prior column using SequentialAccess";
        public const string AttemptToAccessBeforeRead = "Invalid attempt to access a field before calling Read()";
        public const string UnableToStartSecondAsyncOp = "Unable to start a second async operation while one is running.";
        public const string MoreThanOneOPRow = "INTERNAL ERROR:  More than one output parameter row detected.";
        public const string InvalidValueForBoolean = "'{0}' is an illegal value for a boolean option.";
        public const string ServerTooOld = "Connector/Net no longer supports server versions prior to 5.0";
        public const string InvalidConnectionStringValue = "The requested value '{0}' is invalid for the given keyword '{1}'.";
        public const string TraceCloseConnection = "{0}: Connection Closed";
        public const string TraceOpenConnection = "{0}: Connection Opened: connection string = '{1}'";
        public const string TraceQueryOpened = "{0}: Query Opened: {2}";
        public const string TraceResult = "{0}: Resultset Opened: field(s) = {1}, affected rows = {2}, inserted id = {3}";
        public const string TraceQueryDone = "{0}: Query Closed";
        public const string TraceSetDatabase = "{0}: Set Database: {1}";
        public const string TraceUAWarningBadIndex = "{0}: Usage Advisor Warning: Query is using a bad index";
        public const string TraceUAWarningNoIndex = "{0}: Usage Advisor Warning: Query does not use an index";
        public const string TraceResultClosed = "{0}: Resultset Closed. Total rows={1}, skipped rows={2}, size (bytes)={3}";
        public const string TraceUAWarningSkippedRows = "{0}: Usage Advisor Warning: Skipped {2} rows. Consider a more focused query.";
        public const string TraceUAWarningSkippedColumns = "{0}: Usage Advisor Warning: The following columns were not accessed: {2}";
        public const string TraceUAWarningFieldConversion = "{0}: Usage Advisor Warning: The field '{2}' was converted to the following types: {3}";
        public const string TraceOpenResultError = "{0}: Error encountered attempting to open result: Number={1}, Message={2}";
        public const string TraceFetchError = "{0}: Error encountered during row fetch. Number = {1}, Message={2}";
        public const string TraceWarning = "{0}: MySql Warning: Level={1}, Code={2}, Message={3}";
        public const string TraceErrorMoreThanMaxValueConnections = "Unable to trace.  There are more than Int32.MaxValue connections in use.";
        public const string TraceStatementPrepared = "{0}: Statement prepared: sql='{1}', statement id={2}";
        public const string TraceStatementClosed = "{0}: Statement closed: statement id = {1}";
        public const string TraceStatementExecuted = "{0}: Statement executed: statement id = {1}";
        public const string UnableToEnableQueryAnalysis = "Unable to enable query analysis.  Be sure the MySql.Data.EMTrace assembly is properly located and registered.";
public const string TraceQueryNormalized = "{0}: Query Normalized: {2}";
        public const string NoWindowsIdentity = "Cannot retrieve Windows identity for current user. Connections that use  IntegratedSecurity cannot be  pooled. Use either 'ConnectionReset=true' or  'Pooling=false' in the connection string to fix.";
        public const string RoutineRequiresReturnParameter = "Attempt to call stored function '{0}' without specifying a return parameter";
        public const string CanNotDeriveParametersForTextCommands = "Parameters can only be derived for commands using the StoredProcedure command type.";
        public const string ReplicatedConnectionsAllowOnlyReadonlyStatements = "Replicated connections allow only readonly statements.";
        public const string FileBasedCertificateNotSupported = "File based certificates are only supported when connecting to MySQL Server 5.1 or greater.";
        public const string SnapshotNotSupported = "Snapshot isolation level is not supported.";
        public const string TypeIsNotExceptionInterceptor = "Type '{0}' is not derived from BaseExceptionInterceptor";
        public const string TypeIsNotCommandInterceptor = "Type '{0}' is not derived from BaseCommandInterceptor";
        public const string UnknownAuthenticationMethod = "Unknown authentication method '{0}' was requested.";
        public const string AuthenticationFailed = "Authentication to host '{0}' for user '{1}' using method '{2}' failed with message: {3}";
        public const string WinAuthNotSupportOnPlatform = "Windows authentication connections are not supported on {0}";
        public const string AuthenticationMethodNotSupported = "Authentication method '{0}' not supported by any of the available plugins.";
        public const string UnableToCreateAuthPlugin = "Unable to create plugin for authentication method '{0}'. Please see inner exception for details.";
        public const string MixedParameterNamingNotAllowed = "Mixing named and unnamed parameters is not allowed.";
        public const string ParameterIndexNotFound = "Parameter index was not found in Parameter Collection.";
        public const string OldPasswordsNotSupported = "Authentication with old password no longer supported, use 4.1 style passwords.";
        public const string InvalidMicrosecondValue = "Microsecond must be a value between 0 and 999999.";
        public const string InvalidMillisecondValue = "Millisecond must be a value between 0 and 999. For more precision use Microsecond.";
        public const string Replication_NoAvailableServer = "No available server found.";
        public const string Replication_ConnectionAttemptFailed = "Attempt to connect to '{0}' server failed.";
        public const string UnknownConnectionProtocol = "Unknown connection protocol";
        public const string NoUnixSocketsOnWindows = "Unix sockets are not supported on Windows.";
        public const string ReplicationServerNotFound = "Replicated server not found: '{0}'";
        public const string ReplicationGroupNotFound = "Replication group '{0}' not found.";
        public const string NewValueShouldBeMySqlParameter = "The new value must be a MySqlParameter object.";
        public const string ValueNotCorrectType = "Value '{0}' is not of the correct type.";
        public const string CompressionNotSupported = "Compression is not supported.";
        public const string SslConnectionError = "SSL Connection error.";
        public const string OptionNotCurrentlySupported = "The option '{0}' is not currently supported.";
        public const string keywords = @"ACCESSIBLE
ADD
ALL
ALTER
ANALYZE
AND
AS
ASC
ASENSITIVE
BEFORE
BEGIN
BETWEEN
BIGINT
BINARY
BLOB
BOTH
BY
CALL
CASCADE
CASE
CHANGE
CHAR
CHARACTER
CHECK
COLLATE
COLUMN
COMMIT
CONDITION
CONNECTION
CONSTRAINT
CONTINUE
CONVERT
CREATE
CROSS
CURRENT_DATE
CURRENT_TIME
CURRENT_TIMESTAMP
CURRENT_USER
CURSOR
DATABASE
DATABASES
DAY_HOUR
DAY_MICROSECOND
DAY_MINUTE
DAY_SECOND
DEC
DECIMAL
DECLARE
DEFAULT
DELAYED
DELETE
DESC
DESCRIBE
DETERMINISTIC
DISTINCT
DISTINCTROW
DIV
DO
DOUBLE
DROP
DUAL
EACH
ELSE
ELSEIF
ENCLOSED
END
ESCAPED
EXISTS
EXIT
EXPLAIN
FALSE
FETCH
FLOAT
FLOAT4
FLOAT8
FOR
FORCE
FOREIGN
FROM
FULLTEXT
GOTO
GRANT
GROUP
HAVING
HIGH_PRIORITY
HOUR_MICROSECOND
HOUR_MINUTE
HOUR_SECOND
IF
IGNORE
IN
INDEX
INFILE
INNER
INOUT
INSENSITIVE
INSERT
INT
INT1
INT2
INT3
INT4
INT8
INTEGER
INTERVAL
INTO
IS
ITERATE
JOIN
KEY
KEYS
KILL
LABEL
LEADING
LEAVE
LEFT
LIKE
LIMIT
LINEAR
LINES
LOAD
LOCALTIME
LOCALTIMESTAMP
LOCK
LONG
LONGBLOB
LONGTEXT
LOOP
LOW_PRIORITY
MASTER_SSL_VERIFY_SERVER_CERT
MATCH
MEDIUMBLOB
MEDIUMINT
MEDIUMTEXT
MIDDLEINT
MINUTE_MICROSECOND
MINUTE_SECOND
MOD
MODIFIES
NATURAL
NOT
NO_WRITE_TO_BINLOG
NULL
NUMERIC
ON
OPTIMIZE
OPTION
OPTIONALLY
OR
ORDER
OUT
OUTER
OUTFILE
PRECISION
PRIMARY
PROCEDURE
PURGE
RANGE
READ
READS
READ_ONLY
READ_WRITE
REAL
REFERENCES
REGEXP
RELEASE
RENAME
REPEAT
REPLACE
REQUIRE
RESTRICT
RETURN
REVOKE
RIGHT
RLIKE
ROLLBACK
SCHEMA
SCHEMAS
SECOND_MICROSECOND
SELECT
SENSITIVE
SEPARATOR
SET
SHOW
SMALLINT
SPATIAL
SPECIFIC
SQL
SQLEXCEPTION
SQLSTATE
SQLWARNING
SQL_BIG_RESULT
SQL_CALC_FOUND_ROWS
SQL_SMALL_RESULT
SSL
STARTING
STRAIGHT_JOIN
TABLE
TERMINATED
THEN
TINYBLOB
TINYINT
TINYTEXT
TO
TRAILING
TRIGGER
TRUE
UNDO
UNION
UNIQUE
UNLOCK
UNSIGNED
UPDATE
UPGRADE
USAGE
USE
USING
UTC_DATE
UTC_TIME
UTC_TIMESTAMP
VALUE
VALUES
VARBINARY
VARCHAR
VARCHARACTER
VARYING
WHEN
WHERE
WHILE
WITH
WRITE
XOR
YEAR_MONTH
ZEROFILL";
    }
}
