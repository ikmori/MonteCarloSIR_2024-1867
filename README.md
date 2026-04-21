# Simulación Monte-Carlo de Epidemias (Modelo SIR Espacial)

Este repositorio contiene la implementación de una simulación Monte-Carlo basada en el modelo epidemiológico SIR (Susceptible, Infectado, Recuperado/Removido) utilizando un autómata celular sobre una grilla bidimensional de 1000x1000 celdas (1 millón de individuos).

El proyecto está dividido en dos componentes principales para aprovechar al máximo el rendimiento computacional y las capacidades gráficas:
1. **Motor de Simulación (C# .NET):** Encargado de la lógica matemática, la paralelización mediante la *Task Parallel Library* (TPL) y la generación de estados diarios.
2. **Visualización y Análisis (Python):** Encargado de procesar los volcados de memoria binarios generados por el motor para renderizar una animación comparativa side-by-side y gráficas estadísticas.

## Estructura del Proyecto

* `/EngineApp`: Contiene la solución en C# con el motor de simulación.
  * `EngineApp_Secuencial/`: Variante del motor ejecutándose en un solo hilo (baseline).
  * `EngineApp_Paralelo/`: Variante del motor ejecutándose concurrentemente con particionamiento de memoria.
* `/Visualization`: Contiene los scripts de Python (`animate.py`, `plot_scaling.py`) para la generación de recursos visuales.
* `/Data`: Directorio autogenerado donde se almacenan los archivos binarios (`.bin`) de cada fotograma y el archivo de métricas (`scaling.csv`).

## Requisitos Previos

* .NET 8.0 SDK (o superior)
* Python 3.9+
* Paquetes de Python: `numpy`, `matplotlib`, `pandas`
* `ffmpeg` instalado en el sistema operativo (requerido por matplotlib para la exportación a .mp4)

## Instrucciones de Ejecución

### 1. Ejecutar el Motor de Simulación (C#)
El motor de C# generará la data secuencial, la data paralela y finalmente ejecutará un benchmark de *strong scaling* para 1, 2, 4 y 8 núcleos.

```bash
cd EngineApp
dotnet run -c Release
```
*(Nota: Se recomienda usar el modo `Release` para obtener métricas de rendimiento reales).*

### 2. Generar Animación y Gráficas (Python)
Una vez que el motor haya finalizado y generado los archivos en `/Data`, activa tu entorno virtual de Python y ejecuta los scripts:

```bash
cd Visualization
# Para generar el video sir_comparison.mp4:
python animate.py

# Para generar la gráfica de rendimiento speedup_graph.png:
python plot_scaling.py
```
