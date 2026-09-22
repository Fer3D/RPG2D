# Tiny Warrior

<img width="958" height="599" alt="Menú principal" src="https://github.com/user-attachments/assets/6d929728-2212-4411-8fcd-8012b0614d35" />

<img width="958" height="593" alt="Gameplay" src="https://github.com/user-attachments/assets/f3076930-18d0-4378-9f45-965d30088120" />

RPG 2D top-down en pixel art. Eres un caballero chiquito, recoges recursos, matas lo que se cruce y vas completando misiones hasta terminar la historia.

Hecho en Unity 6.

## Cómo se juega

Empiezas en el menú. Puedes elegir el color del personaje (azul, morado, rojo o amarillo), bajar opciones de calidad si hace falta, y darle a New Game. Load Game solo se activa si ya hay un guardado.

Al entrar al mapa te sale un poco de historia y luego te suelta en el pueblo. El objetivo es ir hablando con el NPC de misiones, traerle lo que pida (madera, carne, monedas) y pasar a la siguiente quest. Cuando acabas todas, hay un cierre y vuelves al menú.

Recursos:
- **Madera** — corta árboles (click izquierdo)
- **Carne** — sale de ovejas
- **Monedas** — bolsas por el suelo o drops

Los enemigos te persiguen y te pegan. Al matarlos ganas XP. A los 100 XP subes de nivel (más velocidad, daño y vida). El dash con espacio ayuda a salir de líos.

## Controles

| Tecla | Qué hace |
| --- | --- |
| WASD / flechas | Moverse |
| Click izquierdo | Atacar |
| Espacio | Dash |
| Q | Abrir / cerrar panel de quest |
| I | Inventario |
| U | Stats (nivel, XP, daño, velocidad) |
| Escape | Pausa |
| T | Guardar (en partida) |
| L | Cargar (en partida) |

También puedes guardar desde el menú de pausa.

## Guardado

El save va a un `savefile.json` en la carpeta de datos de Unity (`Application.persistentDataPath`). Guarda posición, vida, nivel, stats, skin y recursos. Si no hay archivo, el botón de Load Game del menú queda gris.

## Abrir el proyecto

1. Unity Hub → Open → esta carpeta
2. Versión: **Unity 6000.2.13f1** (o compatible de la 6)
3. Escena de menú: `Assets/Scenes/MainMenu`
4. Escena de juego: `Assets/Scenes/Level1`

Play desde el editor o build normal de Unity.

## Estructura (lo útil)

```
Assets/
  Scripts/
    Player/          movimiento, ataque, dash, nivel, skins
    Enemy/           IA con NavMesh
    NPC/             comportamiento base (path, huida, chase)
    Quests/          misiones y requisitos de recursos
    UI/              HUD, paneles, historia
    SaveLoadManager/ JSON save/load
    MainMenu/        menú, opciones, skins
  Scenes/
  Prefabs/
```

Pathfinding 2D con NavMeshPlus. Audio, árboles que respawnean, spawner de enemigos y alguna puerta de mina también están por ahí.

## Notas

Proyecto personal / portfolio. No es un MMORPG ni tiene multiplayer. Un mapa, un loop de quests, y listo.
