using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPPP
{
    public partial class InterfazEdicion : Form
    {
        public StreamReader lector;
        PictureBox Hoja;
        int NC;
        private List<Image> imagenes = new List<Image>();

        public InterfazEdicion()
        {

            InitializeComponent();
            openFileDialog1.ShowDialog(this);         
            
}

        private void AbrirImagen()
        {
            try {

            lector = new StreamReader(openFileDialog1.FileName);
            
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = pnPrevisualizacion.Size; // Tamaño de la imagen dentro del panel
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Escala la imagen para ajustarse al PictureBox
            pictureBox.Image = Image.FromFile(openFileDialog1.FileName); // Carga la imagen
            pnPrevisualizacion.Controls.Add(pictureBox);// Agrega el PictureBox al panel
            
            }
            catch{
            //error 
            }
        }



        /*private void BitmapRecortar()
        {
            Rectangle rectOrig = new Rectangle(posXmin, posYmin, anchura, altura);
            Bitmap source = new Bitmap(openFileDialog1.FileName);


            Rectangle rectOrig = new Rectangle(posXmin, posYmin, anchura, altukra);

            Bitmap CroppedImage = CropImage(source, rectOrig);hh

        }
        */
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {

            InterfazPrincipal interfazPrincipal=new InterfazPrincipal();

            // Mostrar el nuevo formulario
            this.Visible = false;
            interfazPrincipal.Show();


        }


        public void TPHoja(int tipoH)
        {
            Size tamañoHoja;
            switch (tipoH)
            {
                case 1: // Carta
                    tamañoHoja = new Size(2550 , 3300); // Tamaño en píxeles (ancho x alto)
                    break;

                case 2: // Oficio
                    tamañoHoja = new Size(2550 , 4200); // Tamaño en píxeles (ancho x alto)
                    break;

                case 3: // A4
                    tamañoHoja = new Size(2480 , 3508); // Tamaño en píxeles (ancho x alto)
                    break;

                // ------------------- 01/05/24 -----------------------------------------------
                case 4:
                    tamañoHoja = new Size(3508, 4961);
                    break;

                case 5:
                    tamañoHoja = new Size(5100, 6600);
                    break;


                default:
                    MessageBox.Show("Tipo de número no válido. Por favor, elija 1 para Carta o 2 para Oficio.");
                    return;
            }

            // Crear el PictureBox para la previsualización de la hoja
            Hoja = new PictureBox();
            
            Hoja.Left = 50;
            Hoja.BackColor = Color.White;
            Hoja.Top = 50;
            Hoja.Size = tamañoHoja; // Tamaño del PictureBox igual al tamaño de la hoja
            Hoja.SizeMode = PictureBoxSizeMode.Zoom; // Escalar la imagen para ajustarse al PictureBox

            // Cargar la imagen de la hoja en el PictureBox
            // (Asegúrate de reemplazar "HojaOriginal" con el nombre de tu imagen)
            //Hoja.Image = Properties.Resources.HojaOriginal; // Cambiar "HojaOriginal" por el nombre de tu imagen
            Hoja.Tag = tamañoHoja; // Almacenar el tamaño original de la imagen en el Tag del PictureBox
            //Hoja.SizeChanged += Hoja_SizeChanged;
            // Agregar el PictureBox al formulario
            this.PanelPre.Controls.Add(Hoja);

            // Agregar controles de zoom (por ejemplo, botones de zoom) al formulario
            // (Agrega aquí los controles que permitirán al usuario hacer zoom en la imagen)
        AbrirImagen(); // SELECCIONAR IMAGEN AL ABRIR LA VENTANA
        }

        // Método para hacer zoom en la imagen de la hoja
        private void ZoomIn(PictureBox pictureBox)
        {
            Size tamañoOriginal = (Size)pictureBox.Tag;
            Size tamañoActual = pictureBox.ClientSize;
            int nuevoAncho = (int)(tamañoActual.Width * 1.1);
            int nuevoAlto = (int)(tamañoActual.Height * 1.1);
            nuevoAncho = Math.Min(nuevoAncho, tamañoOriginal.Width);
            nuevoAlto = Math.Min(nuevoAlto, tamañoOriginal.Height);
            pictureBox.ClientSize = new Size(nuevoAncho, nuevoAlto);
        }

        // Método para alejar la imagen de la hoja
        private void ZoomOut(PictureBox pictureBox)
        {
            Size tamañoActual = pictureBox.ClientSize;
            int nuevoAncho = (int)(tamañoActual.Width / 1.1);
            int nuevoAlto = (int)(tamañoActual.Height / 1.1);
            pictureBox.ClientSize = new Size(nuevoAncho, nuevoAlto);
        }

        // Ejemplo de manejo de eventos para los botones de zoom
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn(Hoja);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut(Hoja);
        }
//
        
        private void NCopias_ValueChanged(object sender, EventArgs e)
        {
            List<Image> imagenesDuplicadas = new List<Image>();
            for (int i = 0; i < NCopias.Value; i++)
            {
                imagenesDuplicadas.AddRange(imagenes);
            }

            // Llamar a AgrImgHoj con las imágenes duplicadas
            AgrImgHoj(imagenesDuplicadas);

        }

        private List<Image> ObtenerImagenesSeleccionadas()
        {
            List<Image> imagenes = new List<Image>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    try
                    {
                        Image imagen = Image.FromFile(filename);
                        imagenes.Add(imagen);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                    }
                }
            }

            return imagenes;
        }

        private void btnAgregarImagenes_Click(object sender, EventArgs e)
        {
            // Este método será llamado cuando el usuario quiera agregar imágenes.
            // Llamamos a ObtenerImagenesSeleccionadas para obtener las imágenes seleccionadas por el usuario.
            imagenes = ObtenerImagenesSeleccionadas();

            // Llamamos a AgrImgHoj con las imágenes obtenidas.
            AgrImgHoj(imagenes);
        }



        private void AgrImgHoj(List<Image> imagenes)
        {
            Hoja.Controls.Clear();

            // Dimensiones de la imagen
            int imagenAncho = 100;
            int imagenAlto = 100;

            // Máximo número de imágenes por fila
            int maxImagenesPorLinea = Hoja.Width / (imagenAncho + 20); // Se agrega un espacio de 20 píxeles entre cada imagen

            // Posición inicial
            int posX = 0;
            int posY = 0;

            foreach (Image imagen in imagenes)
            {
                // Crear un nuevo PictureBox para cada imagen
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(imagenAncho, imagenAlto);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = imagen;

                // Establecer la posición del PictureBox
                pictureBox.Location = new Point(posX, posY);

                // Agregar el PictureBox a Hoja
                Hoja.Controls.Add(pictureBox);

                // Actualizar las coordenadas para la próxima imagen
                posX += imagenAncho + 20; // 20 es el espacio entre cada imagen

                // Si el número de imágenes en la fila actual alcanza el máximo, pasar a la siguiente fila
                if (Hoja.Controls.Count % maxImagenesPorLinea == 0)
                {
                    posX = 0; // Reiniciar la posición X
                    posY += imagenAlto + 20; // Moverse a la siguiente fila
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
      
        }

        private void pnPrevisualizacion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ExportarHojaComoJPG(string rutaArchivo)
        {
            Bitmap hojaBitmap = new Bitmap(Hoja.Width, Hoja.Height);

            using (Graphics g = Graphics.FromImage(hojaBitmap))
            {
                // Dibujamos el fondo blanco
                g.Clear(Color.White);

                foreach (Control control in Hoja.Controls)
                {
                    if (control is PictureBox pictureBox)
                    {
                        // Obtenemos las coordenadas relativas del PictureBox en el contenedor
                        Point pictureBoxLocation = pictureBox.Location;
                        Point pictureBoxRelativeLocation = Hoja.PointToClient(pictureBoxLocation);

                        // Dibujamos la imagen en las coordenadas relativas
                        g.DrawImage(pictureBox.Image, pictureBoxRelativeLocation);
                    }
                }
            }

            hojaBitmap.Save(rutaArchivo, ImageFormat.Jpeg);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Mostrar un cuadro de diálogo para que el usuario elija la ubicación y el nombre del archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de imagen JPG|*.jpg";
            saveFileDialog.Title = "Guardar hoja como JPG";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Llamar al método para exportar la hoja como JPG con la ruta de archivo seleccionada
                ExportarHojaComoJPG(saveFileDialog.FileName);
            }
        }
    } ////////////////////////////////

}

/* // Llamada a este método cuando quieras exportar la hoja como JPG, por ejemplo, desde un botón
 private void btnExportarHoja_Click(object sender, EventArgs e)
 {
     // Mostrar un cuadro de diálogo para que el usuario elija la ubicación y el nombre del archivo
     SaveFileDialog saveFileDialog = new SaveFileDialog();
     saveFileDialog.Filter = "Archivos de imagen JPG|*.jpg";
     saveFileDialog.Title = "Guardar hoja como JPG";
     if (saveFileDialog.ShowDialog() == DialogResult.OK)
     {
         // Llamar al método para exportar la hoja como JPG con la ruta de archivo seleccionada
         ExportarHojaComoJPG(saveFileDialog.FileName);
     }
 }

 private void btnGuardar_Click(object sender, EventArgs e)
 {
     // Mostrar un cuadro de diálogo para que el usuario elija la ubicación y el nombre del archivo
     SaveFileDialog saveFileDialog = new SaveFileDialog();
     saveFileDialog.Filter = "Archivos de imagen JPG|*.jpg";
     saveFileDialog.Title = "Guardar hoja como JPG";
     if (saveFileDialog.ShowDialog() == DialogResult.OK)
     {
         // Llamar al método para exportar la hoja como JPG con la ruta de archivo seleccionada
         ExportarHojaComoJPG(saveFileDialog.FileName);
     }
 }*/