-- Limpando as tabelas antes do teste
DELETE FROM TaskHistories;
DELETE FROM TaskComments;
DELETE FROM Tasks;
DELETE FROM Projects;

-- Projetos criados nos últimos 30 dias
INSERT INTO Projects (Id, Name, Description, UserId, CreatedAt)
VALUES (1, 'Projeto A', 'Projeto com 2 tarefas', 2, DATETIME('now', '-10 days'));

INSERT INTO Projects (Id, Name, Description, UserId, CreatedAt)
VALUES (2, 'Projeto B', 'Projeto com 4 tarefas', 2, DATETIME('now', '-5 days'));

-- Projeto fora dos últimos 30 dias
INSERT INTO Projects (Id, Name, Description, UserId, CreatedAt)
VALUES (3, 'Projeto Antigo', 'Fora dos últimos 30 dias', 2, DATETIME('now', '-40 days'));

-- Tarefas do Projeto A (2 tarefas)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (1, 'Tarefa 1', 'Do projeto A', 'M', 'C', NULL, DATETIME('now', '-9 days'), DATETIME('now', '-7 days'), DATETIME('now'), 1, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (2, 'Tarefa 2', 'Do projeto A', 'B', 'P', NULL, DATETIME('now', '-8 days'), NULL, DATETIME('now'), 1, 2);

-- Tarefas do Projeto B (4 tarefas)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (3, 'Tarefa 1', 'Do projeto B', 'A', 'C', NULL, DATETIME('now', '-4 days'), DATETIME('now', '-2 days'), DATETIME('now'), 2, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (4, 'Tarefa 2', 'Do projeto B', 'M', 'C', NULL, DATETIME('now', '-3 days'), DATETIME('now', '-1 days'), DATETIME('now'), 2, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (5, 'Tarefa 3', 'Do projeto B', 'M', 'P', NULL, DATETIME('now', '-2 days'), NULL, DATETIME('now'), 2, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (6, 'Tarefa 4', 'Do projeto B', 'B', 'P', NULL, DATETIME('now', '-1 days'), NULL, DATETIME('now'), 2, 2);

-- Tarefa do Projeto Antigo (esse não deve contar)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (7, 'Tarefa antiga', 'Projeto fora do intervalo', 'A', 'C', NULL, DATETIME('now', '-39 days'), DATETIME('now', '-38 days'), DATETIME('now'), 3, 2);