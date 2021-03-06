USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_ZONAS_LISTA] 
	@PP_L_DEBUG				INT,
	@PP_K_SISTEMA_EXE		INT
AS
	BEGIN
		SELECT 
			Z.K_ZONA_UO,
			Z.D_ZONA_UO,
			Z.C_ZONA_UO,
			Z.S_ZONA_UO,
			CONVERT(BIT, L_ZONA_UO)
		FROM ZONA_UO Z
		WHERE 
			L_ZONA_UO = 1 AND
			K_ZONA_UO <> 0
		ORDER BY Z.O_ZONA_UO
	END
	

	-- ===============================

