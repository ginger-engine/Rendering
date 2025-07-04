﻿using Engine.Core.Behaviours;
using Engine.Core.Entities;
using Engine.Core.Transform;
using Engine.Rendering.Sprites;

namespace Engine.Rendering.Ui.Label;

public class LabelRenderer : IEntityBehaviour
{
    private readonly RenderQueue _renderQueue;
    private readonly IFontManager _fontManager;

    public LabelRenderer(RenderQueue renderQueue, IFontManager fontManager)
    {
        _renderQueue = renderQueue;
        _fontManager = fontManager;
    }

    public void OnStart(Entity entity)
    {
        UpdateRenderable(entity);

        entity.SubscribeComponentChange<LabelComponent>((_, _) => UpdateRenderable(entity));
        entity.SubscribeComponentChange<TransformComponent>((_, _) => UpdateRenderable(entity));
    }

    public void OnUpdate(Entity entity, float dt)
    {
        _renderQueue.Add(entity.GetComponent<RenderableComponent>().Renderable);
    }

    private void UpdateRenderable(Entity entity)
    {
        var textComponent = entity.GetComponent<LabelComponent>();
        var transform = entity.GetComponent<WorldTransformComponent>();
        var renderableComponent = entity.GetComponent<RenderableComponent>();

        var renderable = new Label
        {
            Text = textComponent.Text,
            Font = _fontManager.Get(textComponent.Font),
            FontSize = textComponent.FontSize,
            Color = textComponent.Color,
            Layer = renderableComponent.Layer,
            Position = transform.Position,
            Rotation = transform.Rotation,
            Scale = transform.Scale
        };

        renderableComponent.Renderable = renderable;
        entity.ApplyComponent(renderableComponent);
    }
}