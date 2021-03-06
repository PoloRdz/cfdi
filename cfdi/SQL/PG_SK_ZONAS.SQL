USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

create PROCEDURE [dbo].[PG_SK_ZONAS] 
	@PP_PAGINA		INT,
	@PP_RPP			INT
AS
	BEGIN
		SELECT 
			Z.K_ZONA_UO,
			Z.D_ZONA_UO,
			Z.C_ZONA_UO,
			Z.S_ZONA_UO,
			CONVERT(BIT, L_ZONA_UO)
		FROM ZONA_UO Z
		ORDER BY K_ZONA_UO
		OFFSET (@PP_PAGINA * @PP_RPP) ROWS FETCH NEXT @PP_RPP ROWS ONLY
	END
	

	-- ===============================

