// Canvas setup
const canvas = document.getElementById('shooting-stars');
const ctx = canvas.getContext('2d');

// Resize canvas to fit the screen
function resizeCanvas() {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
}
window.addEventListener('resize', resizeCanvas);
resizeCanvas();

// ฟังก์ชันวาดดาว
function drawStar(ctx, x, y, spikes, outerRadius, innerRadius, color) {
    const step = Math.PI / spikes; // คำนวณระยะมุมของดาว
    let rotation = -Math.PI / 2; // เริ่มหมุนจากด้านบน

    ctx.beginPath();
    ctx.moveTo(x, y - outerRadius); // จุดเริ่มต้น (ด้านบนของดาว)

    for (let i = 0; i < spikes; i++) {
        // เส้นขอบนอก
        ctx.lineTo(
            x + Math.cos(rotation) * outerRadius,
            y + Math.sin(rotation) * outerRadius
        );
        rotation += step;

        // เส้นขอบใน
        ctx.lineTo(
            x + Math.cos(rotation) * innerRadius,
            y + Math.sin(rotation) * innerRadius
        );
        rotation += step;
    }
    ctx.closePath();
    ctx.fillStyle = color;
    ctx.fill();
}

// ดาวตก
class ShootingStar {
    constructor() {
        this.reset();
    }

    reset() {
        this.x = Math.random() * canvas.width;
        this.y = Math.random() * canvas.height / 2; // เริ่มที่ครึ่งบน
        this.length = Math.random() * 50 + 20; // ความยาวของดาวตก
        this.speed = Math.random() * 2 + 0.5; // ความเร็ว
        this.opacity = Math.random() * 0.7 + 0.3; // โปร่งใส
        this.tailWidth = Math.random() * 3 + 2; // ความกว้างของหาง
        this.angle = Math.PI / 4; // มุมตก
        this.color = `rgba(255, 223, 0, ${this.opacity})`; // สีเหลืองทอง
    }

    draw() {
        // Gradient สำหรับหางดาวตก
        const gradient = ctx.createLinearGradient(
            this.x, this.y,
            this.x - this.length * Math.cos(this.angle),
            this.y - this.length * Math.sin(this.angle)
        );
        gradient.addColorStop(0, this.color);
        gradient.addColorStop(1, 'rgba(255, 255, 255, 0)');

        // วาดหาง
        ctx.beginPath();
        ctx.moveTo(this.x, this.y);
        ctx.lineTo(
            this.x - this.length * Math.cos(this.angle),
            this.y - this.length * Math.sin(this.angle)
        );
        ctx.strokeStyle = gradient;
        ctx.lineWidth = this.tailWidth;
        ctx.stroke();

        // วาดหัวดาวเป็นรูปดาว
        drawStar(ctx, this.x, this.y, 5, this.tailWidth * 2, this.tailWidth, this.color);
    }

    update() {
        this.x += this.speed * Math.cos(this.angle);
        this.y += this.speed * Math.sin(this.angle);

        // รีเซ็ตเมื่อออกนอกจอ
        if (this.x < 0 || this.y > canvas.height) {
            this.reset();
        }
    }
}

// สร้างดาวตกหลายดวง
const stars = [];
for (let i = 0; i < 50; i++) {
    stars.push(new ShootingStar());
}

// Animation loop
function animate() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    for (let star of stars) {
        star.draw();
        star.update();
    }

    requestAnimationFrame(animate);
}
animate();
