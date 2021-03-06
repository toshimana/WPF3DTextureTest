using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPF3DTextureTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Camera Setting
            var camera = new OrthographicCamera();
            camera.Position = new Point3D(0, 0, 1);
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.UpDirection = new Vector3D(0, 1, 0);

            this.Viewport.Camera = camera;

            // Light Setting
            var light = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));
            var lightVisual = new ModelVisual3D();
            lightVisual.Content = light;

            this.Viewport.Children.Add(lightVisual);

            // TextureImage Setting
            int width = 640;
            int height = 640;

            Mat<Vec3b> colorImage = new Mat<Vec3b>(height, width, new Scalar(0, 0, 0));
            Cv2.Rectangle(colorImage, new OpenCvSharp.Rect(width * 1 / 8, height * 1 / 8, width * 3 / 4, height * 3 / 4), new Scalar(255, 0, 0), thickness: -1);
            Cv2.Rectangle(colorImage, new OpenCvSharp.Rect(width * 2 / 8, height * 2 / 8, width * 2 / 4, height * 2 / 4), new Scalar(0, 255, 0), thickness: -1);
            Cv2.Rectangle(colorImage, new OpenCvSharp.Rect(width * 3 / 8, height * 3 / 8, width * 1 / 4, height * 1 / 4), new Scalar(0, 0, 255), thickness: -1);

            // Create MeshModel - TextureCoorinates Range : [0-1]
            {
                var mesh = new MeshGeometry3D();

                mesh.Positions.Add(new Point3D(-0.5, -0.5, 0));
                mesh.Positions.Add(new Point3D(0.5, -0.5, 0));
                mesh.Positions.Add(new Point3D(-0.5, 0.5, 0));

                mesh.Normals.Add(new Vector3D(0, 0, 1));
                mesh.Normals.Add(new Vector3D(0, 0, 1));
                mesh.Normals.Add(new Vector3D(0, 0, 1));

                mesh.TextureCoordinates.Add(new System.Windows.Point(0.0, 1.0));
                mesh.TextureCoordinates.Add(new System.Windows.Point(1.0, 1.0));
                mesh.TextureCoordinates.Add(new System.Windows.Point(0.0, 0.0));

                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(2);

                var bitmapSource = BitmapSourceConverter.ToBitmapSource(colorImage);
                var imageBrush = new ImageBrush(bitmapSource);

                var material = new DiffuseMaterial(imageBrush);

                var geometry = new GeometryModel3D(mesh, material);

                var modelVisual = new ModelVisual3D();
                modelVisual.Content = geometry;

                this.Viewport.Children.Add(modelVisual);
            }

            // Create MeshModel - TextureCoorinates Range : [1/8-7/8]
            {
                var mesh = new MeshGeometry3D();

                mesh.Positions.Add(new Point3D(0.5, -0.5, 0));
                mesh.Positions.Add(new Point3D(0.5, 0.5, 0));
                mesh.Positions.Add(new Point3D(-0.5, 0.5, 0));

                mesh.Normals.Add(new Vector3D(0, 0, 1));
                mesh.Normals.Add(new Vector3D(0, 0, 1));
                mesh.Normals.Add(new Vector3D(0, 0, 1));

                mesh.TextureCoordinates.Add(new System.Windows.Point(7.0 / 8.0, 7.0 / 8.0));
                mesh.TextureCoordinates.Add(new System.Windows.Point(7.0 / 8.0, 1.0 / 8.0));
                mesh.TextureCoordinates.Add(new System.Windows.Point(1.0 / 8.0, 1.0 / 8.0));

                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(2);

                bool isPartialTexture = true;
                if (isPartialTexture)
                {
                    // Set Dummy TextureCoordinates - Range [0-1]
                    mesh.Positions.Add(default(Point3D));
                    mesh.Positions.Add(default(Point3D));

                    mesh.Normals.Add(default(Vector3D));
                    mesh.Normals.Add(default(Vector3D));

                    mesh.TextureCoordinates.Add(new System.Windows.Point(0, 0));
                    mesh.TextureCoordinates.Add(new System.Windows.Point(1, 1));
                }

                var bitmapSource = BitmapSourceConverter.ToBitmapSource(colorImage);
                var imageBrush = new ImageBrush(bitmapSource);

                var material = new DiffuseMaterial(imageBrush);

                var geometry = new GeometryModel3D(mesh, material);

                var modelVisual = new ModelVisual3D();
                modelVisual.Content = geometry;

                this.Viewport.Children.Add(modelVisual);
            }
        }
    }
}
