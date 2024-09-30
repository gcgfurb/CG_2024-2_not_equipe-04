using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Spline : Objeto
    {
        private const int NumeroPontosPorSegmento = 100;  // Número de pontos para desenhar a spline
        private List<Ponto4D> controlPoints;   // Lista de pontos de controle da spline

        public Spline(Objeto _paiRef, ref char _rotulo, List<Ponto4D> pontosControle) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineStrip;  // Usamos LineStrip para desenhar a spline como linhas conectadas

            controlPoints = pontosControle;

            // Adiciona os pontos calculados da spline
            AdicionarPontosSpline();
            Atualizar();
        }

        // Função para adicionar os pontos da spline calculada
        private void AdicionarPontosSpline()
        {
            if (controlPoints.Count < 4)
                throw new ArgumentException("São necessários pelo menos 4 pontos de controle para desenhar uma spline cúbica.");

            // Percorre os pontos de controle em segmentos de 3 (cúbica)
            for (int i = 0; i <= controlPoints.Count - 4; i += 3)
            {
                List<Ponto4D> pontosSegmento = controlPoints.GetRange(i, 4);
                AdicionarPontosBezierSegmento(pontosSegmento);
            }
        }

        // Adiciona os pontos calculados de um segmento Bezier
        private void AdicionarPontosBezierSegmento(List<Ponto4D> pontosControleSegmento)
        {
            for (int i = 0; i <= NumeroPontosPorSegmento; i++)
            {
                double t = (double)i / NumeroPontosPorSegmento;
                Ponto4D ponto = CalcularBezierCubic(t, pontosControleSegmento);
                base.PontosAdicionar(ponto);
            }
        }

        // Função para calcular ponto na curva Bezier cúbica
        private Ponto4D CalcularBezierCubic(double t, List<Ponto4D> pontosControle)
        {
            double x = Math.Pow(1 - t, 3) * pontosControle[0].X +
                       3 * t * Math.Pow(1 - t, 2) * pontosControle[1].X +
                       3 * Math.Pow(t, 2) * (1 - t) * pontosControle[2].X +
                       Math.Pow(t, 3) * pontosControle[3].X;

            double y = Math.Pow(1 - t, 3) * pontosControle[0].Y +
                       3 * t * Math.Pow(1 - t, 2) * pontosControle[1].Y +
                       3 * Math.Pow(t, 2) * (1 - t) * pontosControle[2].Y +
                       Math.Pow(t, 3) * pontosControle[3].Y;

            return new Ponto4D(x, y);
        }

        // Atualiza o objeto no SRU
        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Spline _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
