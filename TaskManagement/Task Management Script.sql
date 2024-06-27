
----------------User Table Begin-----------------
GO
CREATE TABLE [dbo].[TM_Users](nUserId INT PRIMARY KEY IDENTITY(1,1),cName NVARCHAR(100) NOT NULL,cSHName NVARCHAR(6) NOT NULL,
cUserName NVARCHAR(40) NOT NULL,cPassWord NVARCHAR(max) NOT NULL,bCancelled BIT NOT NULL,bActive BIT NOT NULL)
GO

GO
INSERT INTO TM_Users (cName,cSHName,cUserName,cPassWord,bCancelled,bActive)
VALUES ('Demo User','DU','Demo','71dae15333479a59f31089a5cffd4591',0,1)
GO

UserName = Demo
PassWord = Demo@123

----------------Users Table End-----------------

----------------Task Table Begin-----------------
GO
CREATE TABLE [dbo].[TM_Task] (nTaskId INT PRIMARY KEY IDENTITY(1,1),
cTitle NVARCHAR(50), cDecription nvarchar(max) NOT NULL,dDueDate DATETIME NOT NULL,
nStatus INT NOT NULL,cStatus NVARCHAR(50) NOT NULL,nCreatedBy INT NOT NULL,
dCreatedDate DATETIME NOT NULL,nModifiedBy INT,dModifiedDate DATETIME,bCancelled BIT NOT NULL)
GO
----------------Task Table End-----------------


----------------Login Begin------------------
GO
IF  EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[PR_CheckLogin]') AND type IN ( N'P' ))
BEGIN
DROP PROCEDURE [dbo].[PR_CheckLogin]
END
GO
CREATE PROCEDURE [dbo].[PR_CheckLogin]
(
@cUserName NVARCHAR(40),
@cPassWord NVARCHAR(max) 
)
AS
BEGIN

SELECT * FROM TM_Users WHERE cUserName=rtrim(ltrim(@cUserName)) AND cPassWord=rtrim(ltrim(@cPassWord))
AND bCancelled=0 AND bActive=1

END
GO
----------------Login End------------------




----------------Task Save Begin------------------
GO
IF  EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[PR_TaskSave]') AND type IN ( N'P' ))
BEGIN
DROP PROCEDURE [dbo].[PR_TaskSave]
END
GO
CREATE PROCEDURE [dbo].[PR_TaskSave]
(
@nTaskId INT,
@cTitle NVARCHAR(50),
@cDecription NVARCHAR(max),
@cDueDate NVARCHAR(50),
@nStatus INT,
@nUserId INT
)
AS
BEGIN

DECLARE @cStatus NVARCHAR(50)='', @nFlag int

IF(@nStatus=1)
BEGIN
SET @cStatus = 'InCompleted'
END
ELSE
BEGIN
SET @cStatus = 'Completed'
END


IF(@nTaskId=0)
BEGIN
INSERT INTO TM_Task(cTitle, cDecription, dDueDate, nStatus, cStatus, nCreatedBy, dCreatedDate, bCancelled)
VALUES (@cTitle, @cDecription, convert(DATE,@cDueDate,103), @nStatus, @cStatus, @nUserId, getdate(), 0)

SET @nFlag = @@IDENTITY
END

ELSE
BEGIN
UPDATE TM_Task SET cTitle=@cTitle, cDecription=@cDecription, dDueDate=convert(DATE,@cDueDate,103), nStatus=@nStatus,
cStatus=@cStatus, nModifiedBy=@nUserId, dModifiedDate=getdate() WHERE nTaskId=@nTaskId

SET @nFlag = @@ROWCOUNT
END

SELECT @nFlag

END
GO
----------------Task Save End------------------


----------------Task List Begin------------------
GO
IF  EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[PR_TaskList]') AND type IN ( N'P' ))
BEGIN
DROP PROCEDURE [dbo].[PR_TaskList]
END
GO
CREATE PROCEDURE [dbo].[PR_TaskList]
(
@nStatus INT = 0 
)
AS
BEGIN

SELECT TM_Task.*,
FORMAT(TM_Task.dCreatedDate, 'dd/MM/yyyy HH:mm') as  cCreatedDate,
case when TM_Task.dModifiedDate is not null then FORMAT(TM_Task.dModifiedDate, 'dd/MM/yyyy HH:mm') 
else '' end as cModifiedDate,FORMAT(TM_Task.dDueDate, 'dd/MM/yyyy') as cDueDate,
user1.cName as cCreatedBy,case when user2.cName is not null then user2.cName else '' end as cModifiedBy
FROM TM_Task
left join TM_Users user1 on TM_Task.nCreatedby=user1.nUserID
left join TM_Users user2 on TM_Task.nModifiedby=user2.nUserID  
WHERE TM_Task.bCancelled=0 AND (nStatus=@nStatus OR @nStatus=0)

END 
GO
----------------Task List End------------------


----------------Task Delete Begin------------------
GO
IF  EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[PR_TaskDelete]') AND type IN ( N'P' ))
BEGIN
DROP PROCEDURE [dbo].[PR_TaskDelete]
END
GO
CREATE PROCEDURE [dbo].[PR_TaskDelete]
(
@nTaskId INT 
)
AS
BEGIN

UPDATE TM_Task SET bCancelled=1 WHERE nTaskId=@nTaskId
SELECT @@ROWCOUNT

END
GO
----------------Task Delete End------------------
