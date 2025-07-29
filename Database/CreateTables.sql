CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(255) NOT NULL,
    ProviderId NVARCHAR(255) NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT UK_Users_ProviderId_Provider UNIQUE (ProviderId, Provider)
);

CREATE INDEX IX_Users_Email ON Users(Email);

CREATE INDEX IX_Users_Provider ON Users(Provider);

-- Insert sample data (optional)
-- INSERT INTO Users (Email, Name, ProviderId, Provider, CreatedAt, UpdatedAt)
-- VALUES 
--     ('rayahan@gmail.com', 'Rayhan', 'google_123456', 'Google', GETUTCDATE(), GETUTCDATE()),
