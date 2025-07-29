-- Create the Users table for storing user information
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(255) NOT NULL,
    ProviderId NVARCHAR(255) NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Create a unique constraint on ProviderId and Provider combination
    CONSTRAINT UK_Users_ProviderId_Provider UNIQUE (ProviderId, Provider)
);

-- Create an index on Email for faster lookups
CREATE INDEX IX_Users_Email ON Users(Email);

-- Create an index on Provider for faster filtering
CREATE INDEX IX_Users_Provider ON Users(Provider);

-- Insert sample data (optional)
-- INSERT INTO Users (Email, Name, ProviderId, Provider, CreatedAt, UpdatedAt)
-- VALUES 
--     ('john.doe@example.com', 'John Doe', 'google_123456', 'Google', GETUTCDATE(), GETUTCDATE()),
--     ('jane.smith@example.com', 'Jane Smith', 'google_789012', 'Google', GETUTCDATE(), GETUTCDATE());