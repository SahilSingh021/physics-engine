using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace UniverseSim
{
    internal class Renderer
    {
        private SharpDX.Direct3D11.Device? _device;
        private DeviceContext? _deviceContext;
        private SwapChain? _swapChain;
        private RenderTargetView? _renderTargetView;
        private DepthStencilView? _depthStencilView;

        public void Initialize(IntPtr hwnd, int width, int height)
        {
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
        }

        public void Render()
        {
            _deviceContext!.ClearRenderTargetView(_renderTargetView, new RawColor4(0.05f, 0.05f, 0.1f, 1.0f));
            _deviceContext.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);

            _swapChain!.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            _depthStencilView?.Dispose();
            _renderTargetView?.Dispose();
            _swapChain?.Dispose();
            _deviceContext?.Dispose();
            _device?.Dispose();
        }
    }
}