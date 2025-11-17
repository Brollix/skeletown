# Game Design Document (GDD)
## Skeletown

## 1. Visión General
### 1.1 Concepto
Skeletown es un roguelike de acción 2D con elementos de progresión permanente donde los jugadores avanzan a través de habitaciones llenas de enemigos, mejorando su personaje entre partidas para superar desafíos cada vez mayores.

### 1.2 Género
- Acción
- Roguelike
- Progresión permanente
- Combate en tiempo real

### 1.3 Plataformas
- PC (Windows)

### 1.4 Público Objetivo
- Jugadores que disfrutan desafíos difíciles pero justos
- Fanáticos de roguelikes y juegos de acción
- Jugadores que valoran la progresión y mejora constante

## 2. Mecánicas del Juego

### 2.1 Jugabilidad Principal
- **Movimiento**: Control fluido en 8 direcciones
- **Combate**: Disparo con puntería precisa
- **Progresión**: Mejora de atributos entre partidas
- **Muerte y Reintento**: Mecánica de ciclo de juego con progresión permanente

### 2.2 Sistema de Niveles
- **Experiencia (XP)**: Se obtiene al derrotar enemigos
- **Niveles**: Subir de nivel otorga puntos de habilidad
- **Atributos Mejorables**:
  - Vida: Aumenta la salud máxima
  - Daño: Incrementa el daño infligido
  - Velocidad: Mejora la movilidad

### 2.3 Sistema de Combate
- **Ataque Básico**: Disparo de flechas con puntería
- **Patrones de Ataque Enemigos**: Comportamientos únicos por tipo de enemigo
- **Dodge/Esquive**: Movimiento táctico para evitar daño

## 3. Progresión del Juego

### 3.1 Estructura de Niveles
- **Habitaciones**: Niveles independientes con enemigos
- **Progresión**: Avanzar de habitación al limpiar la actual
- **Dificultad**: Aumenta con cada habitación completada

### 3.2 Sistema de Progresión Permanente
- **Puntos de Habilidad**: Se conservan entre partidas
- **Mejoras Permanentes**: Aplican a todas las partidas futuras
- **Equilibrio**: Los enemigos escalan con el nivel del jugador

## 4. Personajes

### 4.1 Jugador
- **Habilidades Básicas**:
  - Movimiento en 8 direcciones
  - Disparo con puntería
  - Esquive

### 4.2 Enemigos
#### 4.2.1 Básico
- Comportamiento: Persigue al jugador
- Ataque: Cuerpo a cuerpo

#### 4.2.2 Tanque
- Alta resistencia
- Movimiento lento
- Mayor daño

#### 4.2.3 Rápido
- Movimiento rápido
- Baja vida
- Ataques rápidos

#### 4.2.4 Jefe Final
- Múltiples fases
- Patrones de ataque complejos
- Recompensa especial al derrotarlo

## 5. Interfaz de Usuario

### 5.1 HUD Principal
- Barra de vida
- Nivel actual
- Experiencia (XP)
- Contador de enemigos restantes
- Indicador de habitación actual

### 5.2 Menús
#### Menú Principal
- Nueva Partida
- Habilidades
- Opciones
- Salir

#### Menú de Pausa
- Reanudar
- Opciones
- Salir al Menú Principal

#### Menú de Habilidades
- Mejoras de atributos
- Descripciones de habilidades
- Previsualización de cambios

## 6. Progresión y Balance

### 6.1 Curva de Dificultad
- **Inicio**: Fácil para aprender controles
- **Mitad**: Requiere mejoras estratégicas
- **Final**: Exige dominio de mecánicas

### 6.2 Fórmulas de Escalado
#### Vida del Enemigo
```
vida_base + (nivel_jugador * 20)
```

#### Daño del Enemigo
```
daño_base * (1 + (nivel_jugador * 0.1))
```

#### Experiencia por Derrota
```
experiencia_base * (1 + (nivel_jugador * 0.2))
```

## 7. Arte y Estilo Visual

### 7.1 Estilo de Arte
- Pixel art retro
- Paleta de colores oscura con acentos brillantes
- Animaciones fluidas

### 7.2 Efectos Visuales
- Partículas para impactos
- Efectos de daño
- Indicadores visuales de mejora

## 8. Audio

### 8.1 Música
- Tema principal atmosférico
- Música de combate intensa
- Música de jefe épica

### 8.2 Efectos de Sonido
- Disparos
- Impactos
- Mejoras
- Muerte de enemigos
- Interfaz de usuario

## 9. Controles
- **WASD/Flechas**: Movimiento
- **Mouse**: Apuntar
- **Clic Izquierdo**: Disparar
- **Espacio**: Esquive/Rodar
- **ESC**: Menú de pausa

## 10. Requisitos Técnicos
- **Sistema Operativo**: Windows 10/11
- **Procesador**: Dual Core 2.4 GHz
- **Memoria**: 4 GB RAM
- **Gráficos**: Tarjeta gráfica con 1GB VRAM
- **Almacenamiento**: 500 MB disponibles

## 11. Plan de Desarrollo

### 11.1 Fase Actual
- Prototipo jugable
- Mecánicas básicas implementadas
- Balance inicial

### 11.2 Próximos Pasos
1. Implementar sistema de habilidades
2. Crear más variedad de enemigos
3. Desarrollar el sistema de sonido
4. Realizar pruebas de jugabilidad
5. Balancear la dificultad

## 12. Equipo
- **Diseño del Juego**: Agustin Brollo
- **Programación**: Agustin Murgia, Franco Chialli
- **Arte**: [Por definir]
- **Música y Sonido**: [Por definir]

---
*Documento actualizado: Noviembre 2025*
*Versión: 1.0*
