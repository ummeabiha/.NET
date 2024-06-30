using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;


//Dtos
public class MaterialDto
{
    public string MaterialName { get; set; }
    public string MaterialStatus { get; set; }
    public List<MaterialPictureDto> MaterialPictures { get; set; }
}

public class MaterialPictureDto
{
    public string PictureName { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }
}

//Models
public class Material
{
    public int MaterialID { get; set; }
    public string MaterialName { get; set; }
    public string MaterialStatus { get; set; }
    public List<MaterialPicture> MaterialPictures { get; set; }
}

public class MaterialPicture
{
    public int PictureID { get; set; }
    public int MaterialID { get; set; }
    public string PictureName { get; set; }
    public string Path { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }
}


class Program
{
    static void Main(string[] args)
    {
        string sqlConnStr = "Server=DESKTOP-3MORQJH;Database=Materials;Integrated Security=True;Encrypt=False;";
        string imagesDirectory = @"C:\Users\laptop world\Downloads\products\"; //will be replaced with form input
        string destDirectory = @"C:\Users\laptop world\Downloads\ConsoleApp1\images\";

        List<MaterialDto> materialsToUpload = GetInputData();

        foreach (var materialDto in materialsToUpload)
        {
            Material material = new Material
            {
                MaterialName = materialDto.MaterialName,
                MaterialStatus = materialDto.MaterialStatus,
                MaterialPictures = new List<MaterialPicture>()
            };

            foreach (var pictureDto in materialDto.MaterialPictures)
            {
                material.MaterialPictures.Add(new MaterialPicture
                {
                    PictureName = pictureDto.PictureName,
                    FileName = pictureDto.FileName,
                    Extension = pictureDto.Extension
                });
            }

            UploadMaterial(material, sqlConnStr, imagesDirectory, destDirectory);
        }
    }

    static List<MaterialDto> GetInputData()
    {
        // Sample Data 
        // will be replaced with form input
        List<MaterialDto> materials = new List<MaterialDto>();

        // Material 1
        MaterialDto material1 = new MaterialDto
        {
            MaterialName = "Material 1",
            MaterialStatus = "Active",
            MaterialPictures = new List<MaterialPictureDto>
            {
                new MaterialPictureDto { PictureName = "1.png", FileName = "1.png", Extension = ".png" },
                new MaterialPictureDto { PictureName = "2.png", FileName = "2.png", Extension = ".png" }
            }
        };
        materials.Add(material1);

        // Material 2
        MaterialDto material2 = new MaterialDto
        {
            MaterialName = "Material 2",
            MaterialStatus = "Inactive",
            MaterialPictures = new List<MaterialPictureDto>
            {
                new MaterialPictureDto { PictureName = "3.png", FileName = "3.png", Extension = ".png" },
                new MaterialPictureDto { PictureName = "4.png", FileName = "4.png", Extension = ".png" }
            }
        };
        materials.Add(material2);

        return materials;
    }

    static void UploadMaterial(Material material, string sqlConnStr, string sourceDirectory, string destinationDirectory)
    {
        using (SqlConnection con = new SqlConnection(sqlConnStr))
        {
            con.Open();

            // Insert material
            string insertMaterialSql = "INSERT INTO Materials (MaterialName, MaterialStatus) OUTPUT INSERTED.MaterialID VALUES (@MaterialName, @MaterialStatus)";
            SqlCommand insertMaterialCmd = new SqlCommand(insertMaterialSql, con);
            insertMaterialCmd.Parameters.AddWithValue("@MaterialName", material.MaterialName);
            insertMaterialCmd.Parameters.AddWithValue("@MaterialStatus", material.MaterialStatus);
            int newMaterialID = (int)insertMaterialCmd.ExecuteScalar();

            // Insert pictures and save files
            string insertPictureSql = "INSERT INTO MaterialPictures (MaterialID, PictureName, Path, FileName, Extension) VALUES (@MaterialID, @PictureName, @Path, @FileName, @Extension)";
            foreach (var picture in material.MaterialPictures)
            {
                string sourceFilePath = Path.Combine(sourceDirectory, picture.FileName);
                string destinationFilePath = Path.Combine(destinationDirectory, picture.FileName);

                if (File.Exists(sourceFilePath))
                {
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    File.Copy(sourceFilePath, destinationFilePath, true);

                    SqlCommand insertPictureCmd = new SqlCommand(insertPictureSql, con);
                    insertPictureCmd.Parameters.AddWithValue("@MaterialID", newMaterialID);
                    insertPictureCmd.Parameters.AddWithValue("@PictureName", picture.PictureName);
                    insertPictureCmd.Parameters.AddWithValue("@Path", destinationFilePath);
                    insertPictureCmd.Parameters.AddWithValue("@FileName", picture.FileName);
                    insertPictureCmd.Parameters.AddWithValue("@Extension", picture.Extension);
                    insertPictureCmd.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine($"File not found: {sourceFilePath}");
                }
            }
        }
    }
}
