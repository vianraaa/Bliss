using Bliss.CSharp.Colors;
using Bliss.CSharp.Effects;
using Bliss.CSharp.Graphics.Pipelines.Buffers;
using Bliss.CSharp.Graphics.Pipelines.Textures;
using Bliss.CSharp.Graphics.VertexTypes;
using Bliss.CSharp.Images;
using Bliss.CSharp.Materials;
using Bliss.CSharp.Textures;
using Veldrid;

namespace Bliss.CSharp;

public static class GlobalResource {
    
    /// <summary>
    /// Provides access to the global graphics device used for rendering operations.
    /// </summary>
    public static GraphicsDevice GraphicsDevice { get; private set; }
    
    /// <summary>
    /// A global point sampler using clamp addressing mode.
    /// </summary>
    public static Sampler PointClampSampler { get; private set; }
    
    /// <summary>
    /// A global linear sampler using clamp addressing mode.
    /// </summary>
    public static Sampler LinearClampSampler { get; private set; }
    
    /// <summary>
    /// A global 4x anisotropic sampler using clamp addressing mode.
    /// </summary>
    public static Sampler Aniso4XClampSampler { get; private set; }
    
    /// <summary>
    /// Gets the default <see cref="ShaderPair"/> used for rendering sprites.
    /// </summary>
    public static ShaderPair DefaultSpriteShaderPair { get; private set; }
    
    /// <summary>
    /// Gets the <see cref="ShaderPair"/> used for rendering primitive shapes.
    /// </summary>
    public static ShaderPair DefaultPrimitiveShaderPair { get; private set; }
    
    /// <summary>
    /// Gets the default <see cref="ShaderPair"/> used for full-screen render passes.
    /// </summary>
    public static ShaderPair DefaultFullScreenRenderPassShaderPair { get; private set; }
    
    /// <summary>
    /// Gets the default <see cref="ShaderPair"/> used for immediate mode rendering operations.
    /// </summary>
    public static ShaderPair DefaultImmediateRendererShaderPair { get; private set; }
    
    /// <summary>
    /// The default <see cref="ShaderPair"/> used for rendering 3D models.
    /// </summary>
    public static ShaderPair LitModelShaderPair { get; private set; }
    /// <summary>
    /// The default <see cref="ShaderPair"/> used for rendering 3D models.
    /// </summary>
    public static ShaderPair UnlitModelShaderPair { get; private set; }

    /// <summary>
    /// The default <see cref="Texture2D"/> used for immediate mode rendering.
    /// </summary>
    public static Texture2D DefaultImmediateRendererTexture { get; private set; }

    /// <summary>
    /// The default <see cref="Texture2D"/> used for rendering 3D models.
    /// </summary>
    public static Texture2D DefaultModelTexture { get; private set; }

    /// <summary>
    /// Initializes global resources.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device to be used for resource creation and rendering.</param>
    public static void Init(GraphicsDevice graphicsDevice) {
        GraphicsDevice = graphicsDevice;
        
        // Default Samplers.
        PointClampSampler = graphicsDevice.ResourceFactory.CreateSampler(new SamplerDescription {
            AddressModeU = SamplerAddressMode.Clamp,
            AddressModeV = SamplerAddressMode.Clamp,
            AddressModeW = SamplerAddressMode.Clamp,
            Filter = SamplerFilter.MinPointMagPointMipPoint,
            LodBias = 0,
            MinimumLod = 0,
            MaximumLod = uint.MaxValue,
            MaximumAnisotropy = 0
        });
        
        LinearClampSampler = graphicsDevice.ResourceFactory.CreateSampler(new SamplerDescription {
            AddressModeU = SamplerAddressMode.Clamp,
            AddressModeV = SamplerAddressMode.Clamp,
            AddressModeW = SamplerAddressMode.Clamp,
            Filter = SamplerFilter.MinLinearMagLinearMipLinear,
            LodBias = 0,
            MinimumLod = 0,
            MaximumLod = uint.MaxValue,
            MaximumAnisotropy = 0
        });
        
        Aniso4XClampSampler = graphicsDevice.ResourceFactory.CreateSampler(new SamplerDescription {
            AddressModeU = SamplerAddressMode.Clamp,
            AddressModeV = SamplerAddressMode.Clamp,
            AddressModeW = SamplerAddressMode.Clamp,
            Filter = SamplerFilter.Anisotropic,
            LodBias = 0,
            MinimumLod = 0,
            MaximumLod = uint.MaxValue,
            MaximumAnisotropy = 4
        });
        
        // Default sprite effect.
        DefaultSpriteShaderPair = new ShaderPair(graphicsDevice, SpriteVertex2D.VertexLayout, "core/shaders/sprite.vert", "core/shaders/sprite.frag");
        DefaultSpriteShaderPair.AddBufferLayout("ProjectionViewBuffer", SimpleBufferType.Uniform, ShaderStages.Vertex);
        DefaultSpriteShaderPair.AddTextureLayout("fTexture");
        
        // Primitive effect.
        DefaultPrimitiveShaderPair = new ShaderPair(graphicsDevice, PrimitiveVertex2D.VertexLayout, "core/shaders/primitive.vert", "core/shaders/primitive.frag");
        DefaultPrimitiveShaderPair.AddBufferLayout("ProjectionViewBuffer", SimpleBufferType.Uniform, ShaderStages.Vertex);
        
        // FullScreenRenderPass effect.
        DefaultFullScreenRenderPassShaderPair = new ShaderPair(graphicsDevice, SpriteVertex2D.VertexLayout, "core/shaders/composite_common.vert", "core/shaders/composite_final.frag");
        DefaultFullScreenRenderPassShaderPair.AddTextureLayout("fTexture");
        
        // ImmediateRenderer effect.
        DefaultImmediateRendererShaderPair = new ShaderPair(graphicsDevice, ImmediateVertex3D.VertexLayout, "core/shaders/immediate_renderer.vert", "core/shaders/immediate_renderer.frag");
        DefaultImmediateRendererShaderPair.AddBufferLayout("MatrixBuffer", SimpleBufferType.Uniform, ShaderStages.Vertex);
        DefaultImmediateRendererShaderPair.AddTextureLayout("fTexture");
        
        // Default model effect.
        LitModelShaderPair = new ShaderPair(graphicsDevice, Vertex3D.VertexLayout, "core/shaders/msh_generic.vert", "core/shaders/msh_lit.frag");
        LitModelShaderPair.AddBufferLayout("MatrixBuffer", SimpleBufferType.Uniform, ShaderStages.Vertex);
        LitModelShaderPair.AddBufferLayout("ColorBuffer", SimpleBufferType.Uniform, ShaderStages.Fragment);
        LitModelShaderPair.AddTextureLayout("fAlbedo");
        LitModelShaderPair.AddTextureLayout("fRough");
        LitModelShaderPair.AddTextureLayout("fMetal");
        LitModelShaderPair.AddTextureLayout("fNormalMap");
        
        // Default model effect.
        UnlitModelShaderPair = new ShaderPair(graphicsDevice, Vertex3D.VertexLayout, "core/shaders/msh_generic.vert", "core/shaders/msh_unlit.frag");
        UnlitModelShaderPair.AddBufferLayout("MatrixBuffer", SimpleBufferType.Uniform, ShaderStages.Vertex);
        UnlitModelShaderPair.AddBufferLayout("ColorBuffer", SimpleBufferType.Uniform, ShaderStages.Fragment);
        UnlitModelShaderPair.AddTextureLayout("fAlbedo");
        
        // Default immediate renderer texture.
        DefaultImmediateRendererTexture = new Texture2D(graphicsDevice, new Image(1, 1, Color.White));
        
        // Default model texture.
        DefaultModelTexture = new Texture2D(graphicsDevice, new Image(1, 1, Color.White));
    }
    
    /// <summary>
    /// Releases and disposes of all global resources.
    /// </summary>
    public static void Destroy() {
        PointClampSampler.Dispose();
        LinearClampSampler.Dispose();
        Aniso4XClampSampler.Dispose();
        DefaultSpriteShaderPair.Dispose();
        DefaultPrimitiveShaderPair.Dispose();
        DefaultFullScreenRenderPassShaderPair.Dispose();
        DefaultImmediateRendererShaderPair.Dispose();
        UnlitModelShaderPair.Dispose();
        LitModelShaderPair.Dispose();
        DefaultImmediateRendererTexture.Dispose();
        DefaultModelTexture.Dispose();
    }
}