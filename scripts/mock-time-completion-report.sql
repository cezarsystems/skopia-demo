-- Limpando as tabelas antes do teste
DELETE FROM TaskHistories;
DELETE FROM TaskComments;
DELETE FROM Tasks;
DELETE FROM Projects;

-- Projetos
INSERT INTO Projects (Id, Name, Description, UserId, CreatedAt)
VALUES (1, 'Projeto Rápido', 'Tasks finalizadas em poucas horas', 2, DATETIME('now', '-10 days'));

INSERT INTO Projects (Id, Name, Description, UserId, CreatedAt)
VALUES (2, 'Projeto Lento', 'Tasks demoradas', 2, DATETIME('now', '-9 days'));

-- Tarefas – Projeto 1: rápidas (2h, 4h)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (1, 'Task Curta 1', 'Finalizada em 2h', 'M', 'C', NULL, DATETIME('now', '-2 days', '-2 hours'), DATETIME('now', '-2 days'), DATETIME('now'), 1, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (2, 'Task Curta 2', 'Finalizada em 4h', 'M', 'C', NULL, DATETIME('now', '-1 days', '-4 hours'), DATETIME('now', '-1 days'), DATETIME('now'), 1, 2);

-- Tarefas – Projeto 2: lentas (2 dias, 4 dias)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (3, 'Task Longa 1', 'Finalizada em 2 dias', 'B', 'C', NULL, DATETIME('now', '-6 days'), DATETIME('now', '-4 days'), DATETIME('now'), 2, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (4, 'Task Longa 2', 'Finalizada em 4 dias', 'A', 'C', NULL, DATETIME('now', '-10 days'), DATETIME('now', '-6 days'), DATETIME('now'), 2, 2);

-- Tarefa registrada que ainda não foi concluída (esse não deve contar)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (5, 'Task Pendente', 'Ainda não concluída', 'M', 'P', NULL, DATETIME('now', '-1 days'), NULL, DATETIME('now'), 2, 2);
