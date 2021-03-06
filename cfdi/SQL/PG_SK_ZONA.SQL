USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_ZONA] 
	@PP_K_ZONA		INT
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
			K_ZONA_UO = @PP_K_ZONA
	END
	

	-- ===============================

