# Système UI - Escape Facility
## Récapitulatif de l'implémentation

### ? Composants implémentés

#### 1. **Barre de détection** (DetectionUI.cs)
- ? Se remplit progressivement quand un garde voit le joueur
- ? Transition de couleur jaune ? rouge
- ? Décroissance automatique quand le joueur échappe à la vue
- ? Détection basée sur la distance et l'angle de vision
- ? Système de raycast pour vérifier la ligne de vue
- ? S'affiche uniquement pendant la détection

#### 2. **Écran "Caught!"** (DetectionUI.cs)
- ? Fond rouge semi-transparent
- ? Effet de flash/pulsation
- ? Texte "CAUGHT!" affiché
- ? Pause automatique du jeu (Time.timeScale = 0)
- ? Se déclenche quand la barre est pleine

#### 3. **Mini-map** (MiniMapCamera.cs)
- ? Caméra orthographique vue de dessus
- ? Suit automatiquement le joueur
- ? Zoom ajustable
- ? Hauteur configurable
- ? Mode suivi activable/désactivable
- ? Détection automatique du joueur par tag

#### 4. **Système de caméras de surveillance** (SecurityCamera.cs)
- ? Rotation automatique dans un arc
- ? Détection du joueur dans un cône de vision
- ? Changement de couleur selon l'état (vert/rouge)
- ? Activation/désactivation individuelle
- ? Feedback visuel avec lumière
- ? Gizmos dans l'éditeur pour visualiser la portée

#### 5. **Interface de contrôle des caméras** (CameraControlUI.cs)
- ? Panel avec liste des caméras
- ? Boutons pour activer/désactiver chaque caméra
- ? Affichage de la vue de la caméra sélectionnée
- ? Indication visuelle de l'état (ON/OFF)
- ? Sélection avec touches numériques (1-9)
- ? Basculement avec touche Tab
- ? Gestion du curseur (visible en mode caméra)

#### 6. **Gestionnaire UI principal** (GameUIManager.cs)
- ? Singleton pour accès global
- ? Coordination de tous les composants UI
- ? Affichage des objectifs
- ? Affichage du statut (Safe/Alert/Caught)
- ? Recherche automatique des composants
- ? Gestion de la mini-map render texture

#### 7. **Utilitaire de setup** (UISetupHelper.cs)
- ? Création automatique de la structure UI
- ? Configuration des composants via Context Menu
- ? Génération de la barre de détection
- ? Génération de l'écran "Caught!"
- ? Génération de la mini-map
- ? Génération du panel de contrôle caméras

---

## ?? Contrôles

| Touche | Action |
|--------|--------|
| **Échap** | Menu pause |
| **Tab** | Ouvrir/fermer le contrôle caméras |
| **1-9** | Sélectionner une caméra spécifique |

---

## ?? Instructions d'installation

### Étape 1: Tags requis
Créer dans Unity (Edit ? Project Settings ? Tags and Layers):
- `Player` - À assigner au joueur
- `Guard` - À assigner aux gardes

### Étape 2: Hiérarchie de base
```
Scene
??? Canvas
?   ??? (Le script UISetupHelper peut créer tout automatiquement)
??? GameUIManager (GameObject vide avec script)
??? MiniMapCamera (GameObject avec script)
??? Guards (tagger avec "Guard")
    ??? Guard1
    ??? Guard2
    ??? ...
```

### Étape 3: Configuration automatique (option facile)
1. Créer un GameObject "UISetupHelper"
2. Ajouter le script `UISetupHelper`
3. Assigner le Canvas principal
4. Dans l'inspecteur: clic droit sur le script ? "Find and Assign References"
5. Ensuite: clic droit ? "Setup UI Components"
6. ? Tout est créé automatiquement!

### Étape 4: Configuration manuelle (option avancée)
Voir le fichier `Docs/UI_System_README.md` pour les détails complets.

---

## ?? Paramètres recommandés

### DetectionUI
```
Detection Speed: 0.5         // Remplissage progressif
Detection Decay Speed: 1.0   // Décroissance rapide
Start Color: Yellow          // Alerte faible
End Color: Red               // Alerte maximale
```

### SecurityCamera
```
Rotation Speed: 20     // Rotation fluide
Rotation Angle: 60           // Arc de 60° de chaque côté
Detection Range: 15      // Portée de 15 mètres
Detection FOV: 90       // Cône de vision de 90°
```

### MiniMapCamera
```
Height: 20      // Suffisamment haut
Camera Size: 10          // Zoom équilibré
Follow Player: true          // Suit le joueur
```

---

## ?? Personnalisation visuelle

### Couleurs recommandées

**Barre de détection:**
- Début: `RGB(255, 255, 0)` - Jaune
- Fin: `RGB(255, 0, 0)` - Rouge

**Écran Caught:**
- Fond: `RGBA(255, 0, 0, 128)` - Rouge semi-transparent
- Texte: `RGB(255, 255, 255)` - Blanc

**Panel caméras:**
- Fond: `RGBA(26, 26, 26, 230)` - Gris foncé
- Bouton ON: `RGB(0, 255, 0)` - Vert
- Bouton OFF: `RGB(255, 0, 0)` - Rouge

---

## ?? Performance

### Optimisations implémentées:
- ? Détection UI visible uniquement quand nécessaire
- ? Raycast pour ligne de vue (pas de calculs inutiles)
- ? Render textures en résolution optimale (256x256 pour mini-map)
- ? Update des caméras seulement quand actives
- ? Recherche de composants en cache

### Considérations:
- Les render textures des caméras peuvent impacter les performances
- Limitez le nombre de caméras de surveillance actives simultanément
- La détection utilise des raycasts chaque frame (acceptable pour ~10 gardes)

---

## ?? Dépannage

### Problème: La barre de détection ne s'affiche pas
**Solution:** 
- Vérifier que les gardes ont le tag "Guard"
- Vérifier que le joueur a le tag "Player"
- Vérifier que detectionBarPanel est assigné

### Problème: Les caméras ne détectent pas le joueur
**Solution:**
- Vérifier le tag "Player"
- Vérifier les LayerMask de détection
- Vérifier la portée de détection (Detection Range)

### Problème: La mini-map est noire
**Solution:**
- Vérifier que la RenderTexture est assignée
- Vérifier le culling mask de la caméra
- Vérifier la hauteur de la caméra

### Problème: Le jeu ne se met pas en pause
**Solution:**
- Le système utilise Time.timeScale = 0
- S'assurer que vos scripts utilisent Time.deltaTime (affecté par timeScale)
- Pour les animations UI, utiliser Time.unscaledDeltaTime

---

## ?? Prochaines étapes suggérées

### Améliorations futures:
- [ ] Ajouter des icônes sur la mini-map (joueur, gardes, caméras)
- [ ] Système de notifications/messages temporaires
- [ ] Barre de santé/stamina du joueur
- [ ] Interface de hacking pour caméras
- [ ] Enregistrement vidéo des caméras
- [ ] Menu de game over avec options (restart/menu)
- [ ] Effets sonores sur détection
- [ ] Vibration du feedback (manette/mobile)

### Intégration avec d'autres systèmes:
```csharp
// Exemple: Intégrer avec le système d'alerte
void OnPlayerDetected()
{
    AIManager.Instance?.RaiseAlarm(transform.position);
    GameUIManager.Instance?.GetDetectionUI().SetCaught();
}
```

---

## ?? Notes techniques

### Architecture:
- **Découplage**: Les composants UI ne dépendent pas directement du code gameplay
- **Flexibilité**: Utilisation de Transform[] au lieu de GuardAI[] pour éviter les dépendances
- **Extensibilité**: Interfaces claires pour ajouter de nouvelles fonctionnalités

### Compatibilité:
- ? Unity 2021.3+
- ? .NET Framework 4.7.1
- ? C# 9.0
- ? Pas de dépendances externes (TextMeshPro optionnel)

---

## ?? Utilisation dans le code

### Accéder à la détection:
```csharp
if (GameUIManager.Instance.GetDetectionUI().IsCaught)
{
 // Logique de game over
}

float detection = GameUIManager.Instance.GetDetectionUI().DetectionLevel;
```

### Changer l'objectif:
```csharp
GameUIManager.Instance.UpdateObjective("Trouvez la salle de contrôle");
```

### Contrôler les caméras:
```csharp
SecurityCamera[] cameras = GameUIManager.Instance
  .GetCameraControlUI()
    .GetSecurityCameras();

foreach (var cam in cameras)
{
    cam.SetCameraActive(false); // Désactiver toutes les caméras
}
```

---

## ? Résultat final

Vous disposez maintenant d'un système UI complet comprenant:
1. ? Barre de détection progressive et immersive
2. ? Écran de capture avec effet visuel
3. ? Mini-map fonctionnelle
4. ? Système de caméras de surveillance
5. ? Interface de contrôle des caméras
6. ? Gestionnaire centralisé
7. ? Outils de configuration rapide

**Le système est prêt à l'emploi et entièrement documenté!** ??
