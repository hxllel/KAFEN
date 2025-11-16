-- Create Database
CREATE DATABASE KafenData;
GO

USE KafenData;
GO

-- TipoUsuario Table
CREATE TABLE TipoUsuario (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Descripcion NVARCHAR (MAX) NOT NULL
);
GO

-- Usuarios Table
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Nombre NVARCHAR (MAX) NOT NULL,
    Apellido_Paterno NVARCHAR (MAX),
    Apellido_Materno NVARCHAR (MAX),
    Correo NVARCHAR (MAX) UNIQUE NOT NULL,
    Contraseña NVARCHAR (MAX) NOT NULL,
    FechaNacimiento DATETIME,
    TipoUsuarioId INT NOT NULL,
    FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario (Id)
);
GO

-- Categoria Table
CREATE TABLE Categoria (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Nombre NVARCHAR (MAX) NOT NULL
);
GO

-- Articulos Table
CREATE TABLE Articulos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    Nombre NVARCHAR (MAX) NOT NULL,
    Descripcion NVARCHAR (MAX),
    Precio DECIMAL(18, 2) NOT NULL,
    Stock INT DEFAULT 0,
    CategoriaId INT NOT NULL,
    VendedorId INT NOT NULL,
    FOREIGN KEY (CategoriaId) REFERENCES Categoria (Id),
    FOREIGN KEY (VendedorId) REFERENCES Usuarios (Id)
);
GO

-- Estatus Table
CREATE TABLE Estatus (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Descripcion NVARCHAR (MAX) NOT NULL
);
GO

-- Pedidos Table
CREATE TABLE Pedidos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    ClienteId INT NOT NULL,
    EstatusId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE (),
    Total DECIMAL(18, 2),
    FOREIGN KEY (ClienteId) REFERENCES Usuarios (Id),
    FOREIGN KEY (EstatusId) REFERENCES Estatus (Id)
);
GO

-- DetalleVenta Table
CREATE TABLE DetalleVenta (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    IdPedido UNIQUEIDENTIFIER NOT NULL,
    IdProductoId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (IdPedido) REFERENCES Pedidos (Id),
    FOREIGN KEY (IdProductoId) REFERENCES Articulos (Id)
);
GO

-- Carrito Table
CREATE TABLE Carrito (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    IdUsuarioId INT NOT NULL,
    IdArticuloId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    FechaAgregado DATETIME DEFAULT GETDATE (),
    FOREIGN KEY (IdUsuarioId) REFERENCES Usuarios (Id),
    FOREIGN KEY (IdArticuloId) REFERENCES Articulos (Id)
);
GO

-- Insert Initial Data
-- Tipos de Usuario (1 = Cliente, 2 = Vendedor)
INSERT INTO TipoUsuario (Descripcion) VALUES ('Cliente');

INSERT INTO TipoUsuario (Descripcion) VALUES ('Vendedor');
GO

-- Insert Initial Estatus
INSERT INTO Estatus (Descripcion) VALUES ('Pendiente');

INSERT INTO Estatus (Descripcion) VALUES ('Confirmado');

INSERT INTO Estatus (Descripcion) VALUES ('Enviado');

INSERT INTO Estatus (Descripcion) VALUES ('Entregado');

INSERT INTO Estatus (Descripcion) VALUES ('Cancelado');
GO

-- Insert Initial Categories
INSERT INTO Categoria (Nombre) VALUES ('Electrónica');

INSERT INTO Categoria (Nombre) VALUES ('Ropa');

INSERT INTO Categoria (Nombre) VALUES ('Alimentos');

INSERT INTO Categoria (Nombre) VALUES ('Libros');
GO

-- Stored Procedure for Product Validation (as referenced in ClienteController)
CREATE PROCEDURE ValidarProducto
    @idusuario INT,
    @idProducto UNIQUEIDENTIFIER
AS
BEGIN
    SELECT a.Id, a.Nombre, a.Precio, a.Stock
    FROM Articulos a
    WHERE a.Id = @idProducto AND a.Stock > 0
END
GO