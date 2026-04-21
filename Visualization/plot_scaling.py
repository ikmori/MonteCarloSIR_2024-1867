import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('/Users/mori/RiderProjects/MonteCarloSIR/Data/scaling.csv')

plt.figure(figsize=(8, 5))
plt.plot(df['Cores'], df['SpeedUp'], marker='o', color='b', label='Speed-Up Real')
plt.plot(df['Cores'], df['Cores'], linestyle='--', color='gray', label='Speed-Up Ideal (Lineal)')

plt.title('Strong Scaling: Simulación Monte-Carlo SIR')
plt.xlabel('Número de Cores')
plt.ylabel('Speed-Up')
plt.xticks(df['Cores'])
plt.grid(True, linestyle=':', alpha=0.7)
plt.legend()

plt.savefig('speedup_graph.png')
print("Gráfica guardada como speedup_graph.png")