using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using UniverseSim.structs;
using Matrix = SharpDX.Matrix;
using Vector3 = SharpDX.Vector3;
using Vector4 = SharpDX.Vector4;

namespace UniverseSim
{
    internal class Renderer
    {
        private SharpDX.Direct3D11.Device? _device;
        private DeviceContext? _deviceContext;
        private SwapChain? _swapChain;
        private RenderTargetView? _renderTargetView;
        private DepthStencilView? _depthStencilView;

        private VertexShader? _vertexShader;
        private PixelShader? _pixelShader;
        private InputLayout? _inputLayout;
        private SharpDX.Direct3D11.Buffer? _vertexBuffer;
        private SharpDX.Direct3D11.Buffer? _constantBuffer;

        private int _width;
        private int _height;

        public void Initialize(IntPtr hwnd, int width, int height)
        {
            _width = width;
            _height = height;

            SwapChainDescription swapChainDesc;
            swapChainDesc.ModeDescription = new ModeDescription(width, height, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            swapChainDesc.SampleDescription = new SampleDescription(1, 0);
            swapChainDesc.Usage = Usage.RenderTargetOutput;
            swapChainDesc.BufferCount = 1;
            swapChainDesc.OutputHandle = hwnd;
            swapChainDesc.IsWindowed = true;
            swapChainDesc.SwapEffect = SwapEffect.Sequential;
            swapChainDesc.Flags = SwapChainFlags.None;

            SharpDX.Direct3D11.Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.None,
                swapChainDesc,
                out _device,
                out _swapChain);

            _deviceContext = _device.ImmediateContext;

            using (var backBuffer = _swapChain.GetBackBuffer<Texture2D>(0))
            {
                _renderTargetView = new RenderTargetView(_device, backBuffer);
            }

            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            using (var depthBuffer = new Texture2D(_device, depthBufferDesc))
            {
                _depthStencilView = new DepthStencilView(_device, depthBuffer);
            }

            _deviceContext.OutputMerger.SetRenderTargets(_depthStencilView, _renderTargetView);
            _deviceContext.Rasterizer.SetViewport(0, 0, width, height, 0.0f, 1.0f);

            LoadShaders();
            CreateBuffers();
        }

        private void LoadShaders()
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile("shaders/Shaders.hlsl", "VSMain", "vs_5_0");
            _vertexShader = new VertexShader(_device, vertexShaderByteCode);

            var pixelShaderByteCode = ShaderBytecode.CompileFromFile("shaders/Shaders.hlsl", "PSMain", "ps_5_0");
            _pixelShader = new PixelShader(_device, pixelShaderByteCode);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            _inputLayout = new InputLayout(_device, signature, new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0)
            });

            vertexShaderByteCode.Dispose();
            pixelShaderByteCode.Dispose();
        }

        private void CreateBuffers()
        {
            var vertices = new[]
            {
                new Vertex(new Vector3( 0.0f,  0.5f, 0.0f), new Vector4(1, 0, 0, 1)),
                new Vertex(new Vector3( 0.5f, -0.5f, 0.0f), new Vector4(0, 1, 0, 1)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(0, 0, 1, 1))
            };

            _vertexBuffer = SharpDX.Direct3D11.Buffer.Create(_device, BindFlags.VertexBuffer, vertices);

            _constantBuffer = new SharpDX.Direct3D11.Buffer(
                _device,
                Utilities.SizeOf<Matrix>(),
                ResourceUsage.Default,
                BindFlags.ConstantBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0);
        }

        public void Render()
        {
            _deviceContext!.ClearRenderTargetView(_renderTargetView, new RawColor4(0.05f, 0.05f, 0.1f, 1.0f));
            _deviceContext.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);

            var world = Matrix.Identity;
            var view = Matrix.LookAtLH(new Vector3(0, 0, -3), Vector3.Zero, Vector3.UnitY);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)_width / _height, 0.1f, 100f);

            var worldViewProj = world * view * proj;
            worldViewProj.Transpose();

            _deviceContext.UpdateSubresource(ref worldViewProj, _constantBuffer);

            _deviceContext.InputAssembler.InputLayout = _inputLayout;
            _deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            _deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Vertex.SizeInBytes, 0));

            _deviceContext.VertexShader.Set(_vertexShader);
            _deviceContext.VertexShader.SetConstantBuffer(0, _constantBuffer);
            _deviceContext.PixelShader.Set(_pixelShader);

            _deviceContext.Draw(3, 0);

            _swapChain!.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            _constantBuffer?.Dispose();
            _vertexBuffer?.Dispose();
            _inputLayout?.Dispose();
            _pixelShader?.Dispose();
            _vertexShader?.Dispose();
            _depthStencilView?.Dispose();
            _renderTargetView?.Dispose();
            _swapChain?.Dispose();
            _deviceContext?.Dispose();
            _device?.Dispose();
        }
    }
}