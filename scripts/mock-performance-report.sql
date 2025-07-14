-- Limpando as tabelas antes do teste
DELETE FROM TaskHistories;
DELETE FROM TaskComments;
DELETE FROM Tasks;
DELETE FROM Projects;

-- Projeto criado pelo PM
INSERT INTO Projects (Id, Name, Description, UserId, LastModified)
VALUES (1, 'Projeto Skopia', 'Projeto base do sistema', 2, DATETIME('now'));

-- Tarefas simuladas dentro de 30 dias

-- Project Manager (3 tarefas)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (1, 'Planejamento', 'Organizar backlog', 'M', 'C', NULL, DATETIME('now', '-10 days'), DATETIME('now', '-2 days'), DATETIME('now'), 1, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (2, 'Definir metas', 'Sprint goal', 'A', 'C', NULL, DATETIME('now', '-15 days'), DATETIME('now', '-5 days'), DATETIME('now'), 1, 2);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (3, 'Encerrar sprint', 'Finalizar ciclo', 'B', 'C', NULL, DATETIME('now', '-8 days'), DATETIME('now', '-1 day'), DATETIME('now'), 1, 2);

-- Agile Master (2 tarefas)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (4, 'Daily meeting', 'Facilitar reunião', 'M', 'C', NULL, DATETIME('now', '-6 days'), DATETIME('now', '-2 days'), DATETIME('now'), 1, 3);

INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (5, 'Retrospectiva', 'Avaliar sprint', 'A', 'C', NULL, DATETIME('now', '-7 days'), DATETIME('now', '-1 day'), DATETIME('now'), 1, 3);

-- Product Owner (1 tarefa)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (6, 'Refinar backlog', 'Estimar estórias', 'B', 'C', NULL, DATETIME('now', '-5 days'), DATETIME('now', '-1 day'), DATETIME('now'), 1, 4);

-- Common User (nenhuma tarefa concluída)
INSERT INTO Tasks (Id, Name, Description, Priority, Status, ExpirationDate, CreatedAt, CompletedAt, LastModified, ProjectId, UserId)
VALUES (7, 'Relatar bug', 'Bug na tela de login', 'M', 'P', NULL, DATETIME('now', '-4 days'), NULL, DATETIME('now'), 1, 5);
