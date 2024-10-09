using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int NumeroPontos = 72;  // Número de pontos para desenhar o círculo
        public double Raio = 0.5;  // Raio do círculo
        public Ponto4D Centro { get; set; }  // Centro do círculo
        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double raio) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineLoop;
            Raio = raio;
            Centro = centro;
            // Adiciona os pontos do círculo a partir do centro
            AdicionarPontosCirculo(centro);                         
            Atualizar();
        }

        private void AdicionarPontosCirculo(Ponto4D centro)
        {
            double anguloIncremento = 2 * Math.PI / NumeroPontos;  
            for (int i = 0; i < NumeroPontos; i++)
            {
                double angulo = i * anguloIncremento;
                double x = centro.X + Raio * Math.Cos(angulo); 
                double y = centro.Y + Raio * Math.Sin(angulo); 
                base.PontosAdicionar(new Ponto4D(x, y));  
            }
        }

        public void Mover(double deslocamentoX, double deslocamentoY, Circulo circuloMaior)
        {
            
            double novoX = Centro.X + deslocamentoX;
            double novoY = Centro.Y + deslocamentoY;

            double xmin = circuloMaior.Centro.X - circuloMaior.Raio + this.Raio;
            double xmax = circuloMaior.Centro.X + circuloMaior.Raio - this.Raio;
            double ymin = circuloMaior.Centro.Y - circuloMaior.Raio + this.Raio;
            double ymax = circuloMaior.Centro.Y + circuloMaior.Raio - this.Raio;

            if (novoX < xmin || novoX > xmax || novoY < ymin || novoY > ymax)
            {
                return;
            }

            double dx = novoX - circuloMaior.Centro.X;
            double dy = novoY - circuloMaior.Centro.Y;
            double distanciaQuadrada = dx * dx + dy * dy;

            double distanciaMaxima = circuloMaior.Raio - this.Raio;
            double distanciaMaximaQuadrada = distanciaMaxima * distanciaMaxima;

            if (distanciaQuadrada <= distanciaMaximaQuadrada)
            {
                Centro = new Ponto4D(novoX, novoY);

                for (int i = 0; i < PontosListaTamanho; i++)
                {
                    var ponto = PontosId(i);
                    double deslocX = deslocamentoX;
                    double deslocY = deslocamentoY;
                    PontosAlterar(new Ponto4D(ponto.X + deslocX, ponto.Y + deslocY, ponto.Z), i);
                }
                ObjetoAtualizar();
            }
            else
            {
                return;
            }
        }


        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
