Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Public Class Form1
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click, CaptureToolStripMenuItem.Click
        Me.Opacity = 0
        Dim area As Rectangle
        Dim capture As System.Drawing.Bitmap
        Dim graph As Graphics
        area = Screen.PrimaryScreen.WorkingArea
        capture = New System.Drawing.Bitmap(area.Width, area.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        graph = Graphics.FromImage(capture)
        graph.CopyFromScreen(area.X, area.Y, 0, 0, area.Size, CopyPixelOperation.SourceCopy)
        PictureBox1.Image = capture
        information()
        AutoSave()

    End Sub
    Public Sub information()
        Me.Opacity = 1


    End Sub
    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click, SaveToolStripMenuItem.Click
        Dim save As New SaveFileDialog
        Try
            save.Title = "Save File"
            save.FileName = "Screenshot"
            '  save.Filter = "png|*.png"
            save.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff"
            If save.ShowDialog = Windows.Forms.DialogResult.OK Then
                PictureBox1.Image.Save(save.FileName, Drawing.Imaging.ImageFormat.Png)
            End If

        Catch ex As Exception

        End Try




    End Sub
    Public Sub AutoSave()
        'Dim SavePath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) & "ScreenShot-" & Now.ToString("ddd_dd_MM_yyyy_hh_mm_ss")

        Dim strfilename = My.Computer.FileSystem.SpecialDirectories.MyPictures & "/BeffsEasyCapture/" & "ScreenShot-" & Now.ToString("ddd_dd_MM_yyyy_hh_mm_ss") & ".png"
        'PictureBox1.Image.Save(SavePath, Drawing.Imaging.ImageFormat.Png)

        ' Me.PictureBox1.Image.Save(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyPictures & "/BeffsEasyCapture/", "ScreenShot-" & Now.ToString("ddd_dd_MM_yyyy_hh_mm_ss" & ".Png")))
        Me.PictureBox1.Image.Save(IO.Path.Combine(strfilename))
        Me.Text = "AutoSaved To: " & strfilename
        PostToImgur(strfilename, "1d1225671adec37")

    End Sub




    '
#Region "Others"

#End Region






    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        My.Computer.Clipboard.Clear()

        My.Computer.Clipboard.SetImage(PictureBox1.Image)


    End Sub


    Private Sub beffseasycapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStripDropDownButton1.Visible = False
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        ' EasyCapSettings.Show()


    End Sub
    Private Shared Sub Main(ByVal args As String())
        '  PostToImgur(PictureBox1, IMGUR_ANONYMOUS_API_KEY)
    End Sub

    Public Shared Function PostToImgur(ByVal imagFilePath As String, ByVal apiKey As String) As String
        Dim imageData As Byte()
        Dim fileStream As FileStream = File.OpenRead(imagFilePath)
        imageData = New Byte(fileStream.Length - 1) {}
        fileStream.Read(imageData, 0, imageData.Length)
        fileStream.Close()
        Const MAX_URI_LENGTH As Integer = 32766
        Dim base64img As String = System.Convert.ToBase64String(imageData)
        Dim sb As StringBuilder = New StringBuilder()
        Dim i As Integer = 0

        While i < base64img.Length
            sb.Append(Uri.EscapeDataString(base64img.Substring(i, Math.Min(MAX_URI_LENGTH, base64img.Length - i))))
            i += MAX_URI_LENGTH
        End While

        Dim uploadRequestString As String = "key=" & apiKey & "&title=" & "imageTitle" & "&caption=" & "img" & "&image=" & sb.ToString()
        Dim webRequest As HttpWebRequest = CType(webRequest.Create("https://api.imgur.com/3/image"), HttpWebRequest)
        webRequest.Method = "POST"
        webRequest.ContentType = "application/x-www-form-urlencoded"
        webRequest.ServicePoint.Expect100Continue = False
        webRequest.Headers("Authorization") = "Client-ID 1d1225671adec37"
        Dim streamWriter As StreamWriter = New StreamWriter(webRequest.GetRequestStream())
        streamWriter.Write(uploadRequestString)
        streamWriter.Close()
        Dim response As WebResponse = webRequest.GetResponse()
        Dim responseStream As Stream = response.GetResponseStream()
        Dim responseReader As StreamReader = New StreamReader(responseStream)
        Return responseReader.ReadToEnd()

        My.Computer.Clipboard.SetText(responseReader.ReadToEnd)
        TextBox1.Text = responseReader.ReadToEnd
    End Function
End Class
