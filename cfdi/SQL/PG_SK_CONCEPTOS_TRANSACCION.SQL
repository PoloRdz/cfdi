USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_CONCEPTOS_TRANSACCION] 
	@PP_ID_FOLIO_R				INT,
	@PP_K_UNIDAD_OPERATIVA		INT
AS
	DECLARE @linked_server VARCHAR(15);
	DECLARE @id_linked_server INT;
	DECLARE @query NVARCHAR(500);
	BEGIN
		SELECT @linked_server = D_LINKED_SERVER, @id_linked_server = ID_LINKED_SERVER FROM LINKED_SERVERS WHERE K_UNIDAD_OPERATIVA = @PP_K_UNIDAD_OPERATIVA;
		SET @query =  N'SELECT idRemision,' + CAST(@id_linked_server AS VARCHAR(15)) + ', DP.UNIDAD, DP.CLAVE_UNIDAD, DP.DESCRIPCION, DP.CLAVE_PROD_SERV, DP.NO_IDENTIFICACION, cant, pu AS valurUnitario, total AS importe, dscto AS descuento, fechaC ' +
						'FROM OPENQUERY(' + @linked_server +', ''SELECT  * FROM Liqsys.remisionesn WHERE folioR = '+ CAST(@PP_ID_FOLIO_R AS NVARCHAR(15)) + ' AND idFolioF = -1 AND edoR = "VALIDO"'') P ' +
						'INNER JOIN DESCRIPCION_PRODUCTOS DP ON DP.PRODUCTO = P.producto';

		EXEC sp_executesql @query;
	END

	-- ===============================

