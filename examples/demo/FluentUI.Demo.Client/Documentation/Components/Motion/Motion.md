---
title: Motion
icon: Resize
route: /Motion
---

# Motion

The FluentCxMotion system provides a unified way to animate UI elements using layouts,
transitions, and behaviors. It acts as a high‑level animation engine built on top of
requestAnimationFrame, coordinating motion updates for all nested <code>MotionGroup</code> and
<code>MotionItem</code> components.

A <code>MotionGroup</code> defines how items are positioned, how they animate between states, and how they react to layout changes.

A <code>MotionItem</code> represents a single animatable element inside the group.

## Examples

{{ MotionExample }}

## API Resizer

{{ API Type=Motion }}


# How Motion Works

At its core, the Motion engine:

* runs a centralized animation loop (via requestAnimationFrame)

* updates all registered MotionGroup instances every frame

* computes layout positions for each MotionItem

* interpolates between previous and new positions using transitions

* applies behaviors and animated layouts

* updates the DOM styles efficiently

This architecture ensures smooth, synchronized animations across all items in a group.

## Layout Animation Types

A layout defines how items are positioned inside a MotionGroup.
There are three categories of layouts, each with a different purpose and animation model.

### Static Layouts

Static layouts compute positions once per frame based solely on the current list of items.
They do not depend on time and do not animate by themselves.

Examples:

<code>Grid</code>
<code>Stack</code>
<code>Cascade</code>
<code>Pin</code>
<code>Snake</code>
<code>StackedRotatingLayout</code>

Characteristics:

deterministic, index‑based positioning

no internal animation

transitions animate the movement between old and new positions

ideal for UI layouts, dashboards, lists, galleries, etc.

### Dynamic Layouts

Dynamic layouts compute positions based on geometry or mathematical structures, and may include behaviors that modify items over time.

Examples:

<code>Fan</code>
<code>Flower</code>
<code>Galaxy</code>
<code>Golden Spiral</code>
<code>Heart</code>
<code>Helix</code>
<code>Orbit</code>

Characteristics:

deterministic geometry (spirals, circles, curves…)

can include behaviors (Pulse, Noise, Orbit, Vortex, Wave…)

still not time‑driven by themselves

transitions animate changes when the layout updates

Dynamic layouts are ideal for expressive, data‑driven, or decorative arrangements.

### Animated Layouts

Animated layouts are time‑driven: they update their geometry every frame using the animation loop.

Examples:

<code>RotatingLayout</code>
<code>WaveAnimatedLayout</code>
<code>HelixAnimatedLayout</code>
<code>OrbitAnimatedLayout</code>
<code>SpiralAnimatedLayout</code>
<code>SunburstAnimatedLayout</code>

Characteristics:

implement an internal timeline (OnTick)

positions evolve continuously over time

transitions blend smoothly between frames

ideal for kinetic visuals, loaders, animated backgrounds, or interactive effects

Animated layouts turn the layout itself into an animation.

Transitions
A transition defines how values interpolate between the previous and new layout states:

* duration
* easing function
* delay
* staggered children
* exit animations

Every time a layout changes, Motion automatically:

* snapshots the previous state
* computes the new layout
* generates animation tracks
* interpolates values over time

This ensures smooth, natural movement between states.

### Behaviors

Behaviors are optional modifiers applied after the layout computation.
They can animate or distort items over time.

Examples:

<code>Pulse</code> (scale oscillation)
<code>Breathing</code> (smooth expansion/contraction)
<code>Noise</code> (random jitter)
<code>Orbit</code> (circular motion)
<code>Vortex</code> (spiral motion)
<code>Wave</code> (sinusoidal motion)

Behaviors can be combined with:

dynamic layouts

animated layouts

They add expressiveness without changing the underlying layout geometry.
