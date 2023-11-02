CREATE TABLE Vectors
(
    ImageId        INT FOREIGN KEY REFERENCES Images(Id),
    VectorPosition INT,
    VectorValue    FLOAT,
    PRIMARY KEY (ImageId, VectorPosition)
);