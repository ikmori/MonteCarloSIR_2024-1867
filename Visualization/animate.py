import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation
import os
from matplotlib.colors import ListedColormap

grid_size = 1000
days = 365
interval = 5
frames_list = [d for d in range(0, days) if d % interval == 0]
if (days - 1) not in frames_list:
    frames_list.append(days - 1)

cmap = ListedColormap(['white', 'red', 'gray'])

def load_grid(day, run_type):
    filepath = f"/Users/mori/RiderProjects/MonteCarloSIR/Data/{run_type}/day_{day:03d}.bin"
    if os.path.exists(filepath):
        with open(filepath, 'rb') as f:
            data = np.fromfile(f, dtype=np.uint8)
            return data.reshape((grid_size, grid_size))
    return np.zeros((grid_size, grid_size), dtype=np.uint8)

fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(12, 6))

im1 = ax1.imshow(np.zeros((grid_size, grid_size)), cmap=cmap, vmin=0, vmax=2)
ax1.set_title('Secuencial')
ax1.axis('off')

im2 = ax2.imshow(np.zeros((grid_size, grid_size)), cmap=cmap, vmin=0, vmax=2)
ax2.set_title('Paralelo')
ax2.axis('off')

def update(frame_index):
    day = frames_list[frame_index]
    grid_seq = load_grid(day, "Sequential")
    grid_par = load_grid(day, "Parallel")

    im1.set_array(grid_seq)
    im2.set_array(grid_par)
    fig.suptitle(f'Simulación SIR - Día {day}')
    return [im1, im2]

ani = animation.FuncAnimation(fig, update, frames=len(frames_list), blit=False)
ani.save('sir_comparison.mp4', writer='ffmpeg', fps=10)