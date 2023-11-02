CREATE
OR
ALTER FUNCTION dbo.SimilarImages(@vector NVARCHAR(max))
    RETURNS TABLE
    AS
    RETURN WITH cteVector AS
    (
    SELECT
    cast ([key] AS INT) AS [VectorPosition],
    cast ([value] AS FLOAT) AS [VectorValue]
    FROM
    OPENJSON(@vector)
    ),
    cteSimilar AS
    (
    SELECT TOP (50)
    v2.Id,
    SUM (v1.[VectorValue] * v2.[VectorValue]) / (
    SQRT(SUM (v1.[VectorValue] * v1.[VectorValue])) * SQRT(SUM (v2.[VectorValue] * v2.[VectorValue]))
    ) as CosineDistance
    FROM
    cteVector v1
    INNER JOIN
    [dbo].[Vectors] v2 ON v1.VectorPosition = v2.VectorPosition
    GROUP BY
    v2.ImageId
    ORDER BY
    cosine_distance DESC
    )
SELECT i.FilePath,
       r.CosineDistance
FROM cteSimilar r
         INNER JOIN
    [dbo].[Images] i
ON i.Id = r.ImageId
    GO