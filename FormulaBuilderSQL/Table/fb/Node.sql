CREATE TABLE [fb].[Node]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[FormulaReferenceId] INT NULL,
	[ParentNodeId] INT NULL,
	[NodeTypeId] INT NOT NULL,
	[Value] NVARCHAR(200) NOT NULL,
	[Position] INT NOT NULL,
	CONSTRAINT [FK_Node_FormulaReferenceId] FOREIGN KEY(FormulaReferenceId) REFERENCES [fb].[Node]([Id]),
	CONSTRAINT [FK_Node_ParentNodeId] FOREIGN KEY(ParentNodeId) REFERENCES [fb].[Node](Id),
	CONSTRAINT [FK_Node_NodeTypeId] FOREIGN KEY(NodeTypeId) REFERENCES [fb].[NodeType]([Id])
)
