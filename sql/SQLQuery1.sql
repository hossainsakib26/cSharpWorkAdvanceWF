CREATE SEQUENCE SQ_DepartmentId
AS BIGINT
START WITH 1 INCREMENT BY 1
MINVALUE 1

SELECT NEXT VALUE FOR SQ_DepartmentId

CREATE PROCEDURE SP_SaveDepartment 
@Id int, 
@Name nvarchar(50), 
@code nvarchar(50)
AS
BEGIN
SET NOCOUNT ON;
INSERT INTO Departments (id, name, code) VALUES (@Id, @Name, @code)
END