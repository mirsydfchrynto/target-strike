# Target Strike 3D - Setup Guide

Selamat datang di **Target Strike 3D**, sebuah simulator latihan menembak First Person Shooter (FPS) yang dibangun dengan arsitektur profesional dan modular di Unity 6.

## 1. Persiapan Scene Otomatis (REKOMENDASI)
Saya telah menyertakan sebuah **Setup Tool** profesional untuk mempercepat pekerjaan Anda:
1. Klik menu **Target Strike > Setup Project** di bar menu atas Unity.
2. Klik tombol **Setup Basic Managers** untuk membuat sistem inti.
3. Klik tombol **Setup Player in Scene** untuk memasukkan player lengkap dengan skripnya.
4. Pilih level yang ingin dibuat (**Setup Level 1/2/3**) untuk menyusun target secara otomatis.

Setelah itu, Anda tinggal menekan tombol **Play** untuk mencoba.

## 2. Fitur Spesial Level 3
Pada **Level_03**, buka komponen `PlayerMovement` pada Player dan:
1. Centang `Enable Auto Movement`.
2. Atur `Auto Move Speed` dan `Auto Move Distance`.
Hal ini akan membuat player bergerak otomatis kanan-kiri sesuai permintaan GDD.

## 3. Sistem Input
Game ini menggunakan **Unity Input System** terbaru.
Asset input berada di `Assets/Input/GameInput.inputactions`.

- **WASD**: Bergerak
- **Mouse**: Melihat sekeliling
- **Klik Kiri**: Menembak
- **R**: Reload
- **Shift**: Sprint
- **Esc**: Pause

## 4. Arsitektur Kode
- **Core**: Menggunakan Singleton pattern untuk Manager agar mudah diakses.
- **Player**: Terpisah antara Input, Movement, dan Shooting (SOLID).
- **Target**: Menggunakan sistem Event untuk memberi tahu LevelManager saat target hancur.
- **UI**: Terpusat pada UIManager yang mendengarkan perubahan data dari Player dan Core.

---
**Dibuat secara otomatis oleh Senior Principal Game Developer AI.**
